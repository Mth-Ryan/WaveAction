<script>
	import DelfaultFooter from "$lib/components/DelfaultFooter.svelte";
	import SimplifiedHeader from "$lib/components/SimplifiedHeader.svelte";

</script>
<div>
    <div class="flex flex-col px-6 md:px-10 min-h-screen max-w-[120ch] mx-auto">
        <SimplifiedHeader />

        <main class="flex-1 py-8 flex">
            <slot />
        </main>

        <DelfaultFooter />
    </div>
</div>