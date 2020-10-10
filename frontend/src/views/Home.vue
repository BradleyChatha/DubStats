<template>
  <div>
    <colour-debug></colour-debug>
    <grid-debug></grid-debug>
  </div>
</template>

<script>
import ColourDebug from "@/components/colour-debug.vue";
import GridDebug from "@/components/grid-debug.vue";

export default {
  components: {
    ColourDebug,
    GridDebug
  }
};
</script>
