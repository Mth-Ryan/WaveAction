<script lang="ts">
	import { ArrowRight, Search } from "lucide-svelte";
    import SpotlightGradient from '$lib/assets/spotlight-gradient.svg';

    let dateFormatOptions: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
    };

    let featuredThreadData = {
        title: "Wave Compiler",
        description: `Creating a compiler from scratch from the lexer to his own virtual machine and bycode assembler`,
        updatedAt: new Date(2023, 1, 20),
    };

    let postsData = [
        {
            thumbUrl: "https://cdn.dribbble.com/users/42044/screenshots/3005807/media/d21d0efa3fc373de652cb6beac874f23.png",
            thread: {
                title: "Aspnet Core"
            },
            slugTitle: "e-commerce-microservice-architecture-with-aspnet-core-kafka-and-grpc",
            title: "E-Commerce microservice architecture with Aspnet Core, Kafka and Grpc",
            createdAt: new Date(2023, 5, 23)
        },
        {
            thumbUrl: "https://codequotient.com/blog/wp-content/uploads/2022/05/Difference-Between-Compiler-And-Interpreter-In-Java.jpg",
            thread: {
                title: "Wave Compiler"
            },
            slugTitle: "creating-a-stack-virtual-machine-for-the-wave-compiler",
            title: "Creating a Stack virtual machine for the Wave Compiler",
            createdAt: new Date(2023, 3, 5)
        },
        {
            thumbUrl: "https://media.licdn.com/dms/image/D5612AQFaqTn2Q7-D1A/article-cover_image-shrink_720_1280/0/1660195253757?e=2147483647&v=beta&t=DlC0CtegN4HZTI7kRSxBU-PRZLmtcWe9hOpHygcB6So",
            thread: {
                title: "By Hand Projects"
            },
            slugTitle: "how-hard-is-to-build-your-own-neural-network",
            title: "How hard is to build your own neural network?",
            createdAt: new Date(2023, 2, 12)
        },
    ];
</script>

<svelte:head>
    <title>Home - Wave Action</title>
</svelte:head>

<div class="flex flex-col gap-14">
    <section class="flex flex-col md:grid grid-cols-2 gap-14">
        <div class="flex flex-col justify-between">
            <div>
                <h1 class="font-light text-slate-100 text-4xl">Welcome</h1>
                <p class="mt-4">
                    Developer-Driven Content ahead, proceed with caution, or not. <br />
                </p>
            </div>

            <div class="mt-6">
                <p class="mb-2">Search what you want to read:</p>
                <form action="/search" method="get" class="group flex items-center gap-2 bg-slate-800 bg-opacity-30 p-2 rounded-lg border border-slate-700">
                    <label class="flex gap-4 w-full">
                        <Search class="text-slate-700" />
                        <input name="q" class="bg-transparent w-full border-none outline-none placeholder:text-slate-700" placeholder="Search" type="search">
                    </label>
                    <button type="submit" class="invisible group-hover:visible">
                        <ArrowRight class="text-slate-500" />
                    </button>
                </form>
            </div>
        </div>

        <a href="/threads/Wave-Compiler" class="featured-thread relative overflow-hidden flex p-4 text-slate-300 visited:text-slate-300 bg-slate-800 bg-opacity-50 border border-slate-700 rounded-xl">
            <div class="flex flex-col justify-between">
                <span class="text-slate-700 mb-4">Featured Thread</span>

                <div>
                    <h2 class="text-2xl text-slate-200 mb-3">{featuredThreadData.title}</h2>
                    <p class="text-sm">{featuredThreadData.description}</p>
                </div>

                <span class="text-slate-700 mt-6">Last update: {featuredThreadData.updatedAt.toLocaleString("en-US", dateFormatOptions)}</span>
            </div>

            <img class="absolute z-negative min-w-full right-0 bottom-0" src={SpotlightGradient} alt="">
        </a>
    </section>

    <section>
        <div class="flex flex-wrap gap-8">
            {#each postsData as post}
            <a href={`/posts/${post.slugTitle}`} class="post bg-slate-800 bg-opacity-60 text-slate-300 visited:text-slate-300 rounded-xl border border-slate-700 overflow-hidden flex-[1_32ch] flex flex-col justify-between">
                <div>
                    <div class="w-full bg-slate-700 bg-opacity-50">
                        <div class="aspect-[21/9] bg-cover bg-center" style={`background-image: url('${post.thumbUrl}');`}>
                        </div>
                    </div>

                    <div class="p-4">
                        <div class="mb-3">
                            <span class="bg-slate-700 rounded-full px-2 py-1 text-xs">{post.thread.title}</span>
                        </div>
                        <h3 class="text-slate-100">
                            {post.title}
                        </h3>
                    </div>
                </div>

                <div class="px-4 pb-4">
                    <span class="text-slate-700">{post.createdAt.toLocaleDateString("en-US", dateFormatOptions)}</span>
                </div>
            </a>
            {/each}
        </div>

        <div class="flex items-center justify-center mt-14">
            <a href="/posts">
                <button class="elevated-button">
                    See all our posts
                </button>
            </a>
        </div>
    </section>
</div>

<style lang="postcss">
    .featured-thread:hover {
		box-shadow: 0 0 0.725rem theme(colors.slate.800);
    }

    .post:hover {
		box-shadow: 0 0 0.725rem theme(colors.slate.800);
    }
</style>