<script lang="ts">
    import ErrorIcon from '$lib/assets/error-icon.svg'
    import { page } from '$app/stores';
    
    let error = $page.error?.message;
</script>

<svelte:head>
    <title>{error} - Wave Action</title>
</svelte:head>

<div class="flex flex-col justify-center items-center text-center">
    <img class="w-20 mb-4" alt="" src={ErrorIcon} />

    <h1 class="text-3xl text-slate-200">Oops! {$page.error?.message}</h1>
    
    <p class="mt-4 mb-16">
        This is not the page you're looking for. <br />
        Sorry about that.
    </p>
    
    <a href="/">
        <button class="elevated-button">
            Go Back to the Home Page
        </button>
    </a>
    <span class="block mt-3 text-xs">
        Do you think this is a bug? 
        <a href="https://github.com/Mth-Ryan/WaveAction/issues">open an issue on our project</a>.
    </span>
</div>

