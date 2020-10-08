<template>
  <div>
    <header>
      <nav id="nav">
        <router-link to="/">Home</router-link>
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
