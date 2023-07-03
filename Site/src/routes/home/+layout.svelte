<script>
	import DefaultHeader from '$lib/components/DefaultHeader.svelte';
	import DelfaultFooter from '$lib/components/DelfaultFooter.svelte';
</script>

<div class="flex flex-col px-6 md:px-10 min-h-screen max-w-[120ch] mx-auto">
	<DefaultHeader />

	<main class="flex-1 py-8">
		<slot />
	</main>

	<DelfaultFooter />
</div>
