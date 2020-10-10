<template>
  <div>
    <packages-home></packages-home>
    <colour-debug></colour-debug>
    <grid-debug></grid-debug>
  </div>
</template>

<script>
import PackagesHome from "@/components/packages-home";
import ColourDebug from "@/components/colour-debug.vue";
import GridDebug from "@/components/grid-debug.vue";

export default {
  components: {
    ColourDebug,
    GridDebug,
    PackagesHome
  }
};
</script>
