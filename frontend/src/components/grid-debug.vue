<template>
  <div class="grid container">
    <div class="grid column spanning-1" v-for="colClass in colourClasses" :key="colClass">
      <div :class="colClass + ' box'"></div>
    </div>
  </div>
</template>

<script>
import { defineComponent } from "vue";

export default defineComponent({
  name: "GridDebug",

  data: function() {
    return {
      colourClasses: [
        "primary-100",
        "primary-200",
        "primary-300",
        "primary-400",
        "primary-500",
        "primary-600",
        "primary-700",
        "primary-800",
        "primary-900",
        "accent-100",
        "accent-200",
        "accent-300"
      ]
    };
  }
});
</script>

<style scoped lang="scss">
@import "@/styles/global.scss";

.box {
  width: 100%;
  height: 100%;
}

.container {
  height: 100%;
  min-height: 50px;
}

@each $colour, $values in $COLOURS {
  @if type-of($values) == map {
    @each $weight, $weight-value in $values {
      .#{$colour}-#{$weight} {
        background-color: $weight-value;
      }
    }
  }
}
</style>
