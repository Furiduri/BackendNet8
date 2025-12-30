<script setup lang="ts">
const { locale, setLocale } = useI18n()

function toggleLocale() {
  setLocale(locale.value === 'es' ? 'en' : 'es')
}
</script>

<template>
  <button class="!outline-none flex items-center gap-1" @click="toggleLocale">
    <div class="i-carbon-language" />
    <span class="text-sm uppercase">{{ locale }}</span>
  </button>
</template>
