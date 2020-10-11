<template>
  <div>
    <header>
      <nav class="main navbar grid container">
        <div class="grid column spanning-2"></div>
        <router-link class="centered grid column navbar link spanning-1" to="/"
          >Home</router-link
        >
        <a
          class="centered grid column navbar link spanning-1"
          href="/ui/playground"
          >GraphQL</a
        >
        <div class="grid column spanning-5"></div>
        <div class="grid centered column spanning-3">
          <input
            id="search-bar"
            type="text"
            placeholder="Goto package"
            @keyup.enter="onGotoPackage"
            ref="searchBar"
          />
        </div>
        <div class="grid column spanning-2"></div>
      </nav>
    </header>
    <main class="grid container">
      <div class="grid column spanning-2"></div>
      <router-view class="grid column spanning-8" :key="$route.fullPath" />
      <div class="grid column spanning-2"></div>
    </main>
  </div>
</template>

<script>
export default {
  methods: {
    onGotoPackage: function() {
      this.$router.push("/package/" + this.$refs.searchBar.value);
    }
  }
};
</script>

<style lang="scss">
@import "./styles/sideeffects.scss";

#search-bar {
  height: 30px;
  border-radius: 10px;
  padding-left: 0.5em;
}
</style>
