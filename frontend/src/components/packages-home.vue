<template>
  <div class="wrappable grid container"></div>
</template>

<script>
import GraphQL from "@/lib/graphql";
import { onBeforeMount } from "vue";

export default {
  data: function() {
    return {
      packages: []
    };
  },

  beforeMount: function() {
    GraphQL.query(
      `query {
        
    }`,
      null
    );
  }
};
</script>
