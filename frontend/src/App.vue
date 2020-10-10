<template>
  <div>
    <header>
      <nav class="main navbar grid container">
        <div class="grid column spanning-2"></div>
        <router-link class="centered grid column navbar link spanning-1" to="/">Home</router-link>
        <a class="centered grid column navbar link spanning-1" href="/ui/playground">GraphQL</a>
        <div class="grid column spanning-2"></div>
      </nav>
    </header>
    <main class="grid container">
      <div class="grid column spanning-2"></div>
      <router-view class="grid column spanning-8" />
      <div class="grid column spanning-2"></div>
    </main>
  </div>
</template>

<style lang="scss">
@import "./styles/sideeffects.scss";
</style>
