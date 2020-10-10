<template>
  <div>
    <header>
      <nav class="main navbar grid container">
        <div class="grid column spanning-3"></div>
        <router-link class="centered grid column navbar link spanning-1" to="/">Home</router-link>
        <a class="centered grid column navbar link spanning-1" href="/ui/playground">GraphQL</a>
        <div class="grid column spanning-3"></div>
      </nav>
    </header>
    <main>
      <router-view />
    </main>
  </div>
</template>

<style lang="scss">
@import "./styles/sideeffects.scss";
</style>
