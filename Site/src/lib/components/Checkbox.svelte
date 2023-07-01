<script lang="ts">
	import { Check } from "lucide-svelte";

    export let checked = false;
</script>

<label class="block p-[0.1rem] cursor-pointer text-slate-700 bg-slate-700 border-2 border-slate-600 rounded-sm" class:checked>
    <Check size=10 strokeWidth=4 />
    <input type="checkbox" class="hidden" bind:checked>
</label>

<style lang="postcss">
    label:hover {
		box-shadow: 0 0 0.5rem theme(colors.slate.600);
    }

    .checked {
        @apply bg-slate-200;
        @apply border-slate-200;
    }

    .checked:hover {
		box-shadow: 0 0 0.5rem theme(colors.slate.200);
    }
</style>