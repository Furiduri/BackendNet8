<template>
  <main>
      <Navbar :items="itemsNav" />
      <slot />
      <Footer />
  </main>
</template>

<script lang="ts" setup>
const itemsNav = [
    {
        title: 'home',
        icon: 'i-mdi-home',
        to: '/',
        items: []
    },
    {
        title: 'about',
        icon: 'i-mdi-information',
        to: '/about',
        items: []
    },
    {
        title: 'contact',
        icon: 'i-mdi-phone',
        to: '/contact',
        items: []
    },
]
</script>
