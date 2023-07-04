import { IdentityCookie, RefreshCookie } from "$lib/configConstants";
import type { JWTPayload } from "$lib/models/JWTPayload";
import type { Tokens } from "$lib/models/Tokens";
import { getApiClient } from "$lib/services/apiClient";
import type { Cookies } from "@sveltejs/kit";
import type { AxiosError, AxiosInstance } from "axios";

type RefreshTokenKind = "browser_session" | "long_session";

type RefreshToken = { token: string, kind: RefreshTokenKind };

export class IndentityContext {
    private cookiesService: Cookies;
    private apiClient: AxiosInstance;

    constructor(cookies: Cookies) {
        this.cookiesService = cookies;
        this.apiClient = getApiClient();
    }

    private baseCookieOpts = {
        path: "/",
        httpOnly: true,
        sameSite: "strict" as "strict",
        secure: false,
    }

    private browserRefreshOpts = { ...this.baseCookieOpts }

    private longRefreshOpts = { 
        maxAge: 60 * 60 * 24 * 28, // 28 days
        ...this.baseCookieOpts,
    }

    private identityOpts = {
        maxAge: 60 * 12, //  12 minutes
        ...this.baseCookieOpts,
    }

    private get refreshToken(): RefreshToken | undefined {
        const cookieVal = this.cookiesService.get(RefreshCookie);
        if (cookieVal != undefined) {
            return JSON.parse(cookieVal) as RefreshToken;
        }
        return undefined;
    }

    private set refreshToken(input: RefreshToken) {
        this.cookiesService.set(
            RefreshCookie,
            JSON.stringify(input),
            input.kind == "browser_session" ? this.browserRefreshOpts : this.longRefreshOpts
        );
    }

    private get identityToken(): string | undefined {
        return this.cookiesService.get(IdentityCookie);
    }

    private set identityToken(token: string) {
        this.cookiesService.set(IdentityCookie, token, this.identityOpts);
    }

    public async login(userNameOrEmail: string, password: string, keepLogged: boolean) {
        try {
            const response = await this.apiClient.post("/Access/Login", { userNameOrEmail, password });
            const tokens = response.data as Tokens;
            this.identityToken = tokens.jwt;
            this.refreshToken = { 
                token: tokens.refresh,
                kind: keepLogged ? "long_session" : "browser_session"
            };
            return null;
        } catch (error) {
            return error as AxiosError<any, any>;
        }
    }

    public async signup(data: {
        userName: string,
        email: string,
        password: string,
        profile: {
            firstName: string,
            lastName: string
        }
    }) {
        try {
            const response = this.apiClient.post("/Access/Signup", data);
            const tokens = (await response).data as Tokens;
            this.identityToken = tokens.jwt;
            this.refreshToken = { 
                token: tokens.refresh,
                kind: "long_session"
            };
            return null;
        } catch (error) {
            return error as AxiosError<any, any>;
        }
    }

    public logout() {
        this.cookiesService.set(RefreshCookie, "", { maxAge: -1 })
        this.cookiesService.set(IdentityCookie, "", { maxAge: -1 })
    }

    public async refreshSession() {
        if (this.refreshToken == undefined)
            return new Error("Refresh token not found");

        try {
            const response = await this.apiClient.post("/Access/Refresh", { refresh: this.refreshToken })
            const tokens = response.data as Tokens;
            const currentKind = this.refreshToken!.kind;

            this.identityToken = tokens.jwt;
            this.refreshToken = { token: tokens.refresh, kind: currentKind };
            return null
        } catch (error) {
            return error as AxiosError<any, any>;
        }
    }

    public async isLogged() {
        if (this.identityToken == undefined) {
            const err = await this.refreshSession();
            return err == null;
        }
        return true;
    }

    public async getUserIdentityToken() {
        if (await this.isLogged()) {
            return this.identityToken;
        }
        return undefined;
    }

    public async getUserClaims() {
        const token = await this.getUserIdentityToken();
        if (token == undefined) {
            return undefined;
        }
        return JSON.parse(atob(token.split(".")[1])) as JWTPayload;
    }
}