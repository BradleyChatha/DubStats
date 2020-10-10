<template>
  <div>
    <colour-debug></colour-debug>
    <grid-debug></grid-debug>
  </div>
</template>

<script>
import ColourDebug from "@/components/ColourDisplay.vue";
import GridDebug from "@/components/GridDisplay.vue";

export default {
  components: {
    ColourDebug,
    GridDebug
  }
};
</script>
