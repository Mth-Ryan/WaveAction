<script>
	import '../app.css';
	import BackgroundGradient from '$lib/assets/background-gradient.svg';
	import Logo from '$lib/assets/wave-action-logo.svg';
	import { Search, Menu } from 'lucide-svelte';
	import Sidebar from '$lib/components/Sidebar.svelte';
	import SearchMenu from '$lib/components/SearchMenu.svelte';

	let sidebar = false;
	let searchMenu = false;
</script>

<img class="absolute top-0 left-0 z-negative max-h-screen" src={BackgroundGradient} alt="" />

<div class="flex flex-col px-6 md:px-10 min-h-screen max-w-[120ch] mx-auto">
	<SearchMenu bind:open={searchMenu} />
	<Sidebar bind:open={sidebar} />

	<header class="py-4 md:py-6 flex md:grid grid-cols-[9rem,auto,9rem] items-center justify-between ">
		<a href="/">
			<img class="h-10" src={Logo} alt="Wave Action" />
		</a>

		<nav class="hidden md:flex gap-4 text-xl font-light justify-center">
			<a class="text-slate-200 visited:text-slate-200 px-3 py-1 rounded-md hover:bg-slate-900" href="/">
				Home
			</a>
			<a class="text-slate-200 visited:text-slate-200 px-3 py-1 rounded-md hover:bg-slate-900" href="/posts">
				Posts
			</a>
			<a class="text-slate-200 visited:text-slate-200 px-3 py-1 rounded-md hover:bg-slate-900" href="/threads">
				Threads
			</a>
			<a class="text-slate-200 visited:text-slate-200 px-3 py-1 rounded-md hover:bg-slate-900" href="/about">
				About
			</a>
		</nav>

		<div class="flex gap-6 text-slate-200 justify-end text-xl">
			<button on:click={() => { searchMenu = true; }}>
				<Search />
			</button>
			<button class="md:hidden" on:click={() => { sidebar = true; }}>
				<Menu />
			</button>
		</div>
	</header>

	<main class="flex-1 py-8">
		<slot />
	</main>

	<footer class="flex flex-col items-center gap-1 py-6 border-t border-slate-800">
		<div class="flex gap-3">
			<span>&copy; 2023 - Mateus Ryan</span>
			<span class="text-slate-800">&#x2022;</span>
			<a class="text-slate-300 visited:text-slate-300 hover:text-sky-700" href="https://github.com/Mth-Ryan" target="_blank">
				Github
			</a>
			<span class="text-slate-800">&#x2022;</span>
			<a class="text-slate-300 visited:text-slate-300 hover:text-sky-700" href="https://linkedin.com/in/mateus-ryan-aa63b3210" target="_blank">
				Linkedin
			</a>
		</div>
		<span>Licenced under <a class="visited:text-sky-700" href="https://www.gnu.org/licenses/agpl-3.0.html">AGPL-3.0</a></span>
	</footer>
</div>

<style lang="postcss">
	:global(html) {
		background-color: theme(colors.slate.950);
        color: theme(colors.slate.300);
	}

	:global(body) {
		width: theme(width.screen);
		height: theme(height.screen);
	}
	
	:global(a) {
		@apply text-sky-700;
	}
	
	:global(a:visited) {
		@apply text-sky-900;
	}

	:global(.z-negative) {
		z-index: -1;
	}
	
	:global(.elevated-button) {
		@apply bg-slate-200;
		@apply text-slate-950;
		@apply font-bold;
		@apply px-5;
		@apply py-2;
		@apply rounded-md;
	}
	
	:global(.elevated-button:hover) {
		box-shadow: 0 0 0.5rem theme(colors.slate.300);
	}
</style>
