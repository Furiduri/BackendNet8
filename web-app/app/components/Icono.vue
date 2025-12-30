<script lang="ts" setup>
const props = defineProps<{ NameIcon: string }>()
const darkName = computed(() => `dark:${props.NameIcon}`)
</script>

<template>
    <div class="mx-1" :class="`${NameIcon} ${darkName}`" />
</template>