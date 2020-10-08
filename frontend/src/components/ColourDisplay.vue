<template>
  <div class="grid wrapping container">
    <div class="box grid column span-1" v-for="colClass in colourClasses" :key="colClass">
      <div :class="colClass+' container'">
        <p class="high">{{ colClass }}</p>
        <p class="medium">{{ colClass }}</p>
        <p class="low">{{ colClass }}</p>
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent } from "vue";

export default defineComponent({
  name: "ColourDebug",

  data: function() {
    return {
      colourClasses: [
        "background-0",
        "background-1",
        "background-2",
        "background-3",
        "background-4",
        "background-5",
        "background-6",
        "background-7",
        "background-8",
        "background-9",
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
        "accent-300",
        "accent-400",
        "accent-500",
        "accent-600",
        "accent-700",
        "accent-800",
        "accent-900"
      ]
    };
  }
});
</script>

<style scoped lang="scss">
@import "@/styles/global.scss";

.box,
.container:not(.grid) {
  width: 15vw;
  height: 135px;

  margin: 0 0 1em 1em;

  p {
    text-align: center;

    &.high {
      color: get-colour("text-on-dark", "high");
    }
    &.medium {
      color: get-colour("text-on-dark", "medium");
    }
    &.low {
      color: get-colour("text-on-dark", "low");
    }
  }
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
