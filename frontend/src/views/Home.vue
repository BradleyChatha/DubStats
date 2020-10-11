<template>
  <div>
    <p>
      Don't see a certain package? Type it's name into the search bar, and
      DubStats will start to track it.
    </p>
    <p>
      Stats of "-1" indicate that those stats haven't been fetched yet.
    </p>
    <packages-home></packages-home>
  </div>
</template>

<script>
import PackagesHome from "@/components/packages-home";

export default {
  components: {
    PackagesHome
  }
};
</script>
