<script lang="ts">
	import { Search } from "lucide-svelte";
    import Dialog from "./Dialog.svelte";
	import { goto } from "$app/navigation";

    export let open = false;
    let value = '';

    const handleSubmit = () => {
        goto(`/search?q=${encodeURI(value)}`)
    }

    const handleKey = (e: KeyboardEvent) => {
        switch (e.key) {
            case "Enter":
                handleSubmit();
                break;

            case "Escape":
                open = false;
                break;
        
            default:
                break;
        }
    }
</script>

<Dialog bind:open={open} showHeader={false}>
    <span slot="header">
        Search
    </span>

    <div slot="body">
        <label class="flex gap-2">
            <Search size=20 />
            <input 
                bind:value
                on:keydown={handleKey}
                class="bg-transparent border-none outline-none w-full" 
                type="search"
                placeholder="Search" 
            />
            <span class="bg-slate-700 rounded-md px-2">
                &#10550;
            </span>
        </label>
    </div>
</Dialog>