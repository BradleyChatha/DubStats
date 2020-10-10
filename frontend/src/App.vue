<template>
  <div>
    <header>
      <nav class="main navbar grid container">
        <div class="grid column spanning-4"></div>
        <router-link class="centered grid column navbar link" to="/">Home</router-link>
        <div class="grid column spanning-4"></div>
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
