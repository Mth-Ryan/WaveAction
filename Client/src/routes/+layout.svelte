<script>
	import '../app.css';
</script>

<slot />

<style lang="postcss">
	:global(html) {
		background-color: theme(colors.slate.950);
        color: theme(colors.slate.300);
	}
</style>
