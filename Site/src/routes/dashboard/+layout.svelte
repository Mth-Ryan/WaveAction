<script>
	import DashSidebar from "$lib/components/DashSidebar.svelte";
    import { DashboardLinkName } from "$lib/stores/DashboardLinkNameStore";

    let link = "dashboard";

    DashboardLinkName.subscribe(value => {
        link = value;
    })
</script>

<div class="w-full h-full max-h-screen flex">
    <DashSidebar mainLink={link} />
    <main class="w-full max-h-screen overflow-y-scroll">
        <slot />
    </main>
</div>