<template>
  <section class="wrapping grid container">
    <h1 class="centered grid column spanning-12">
      Packages week of {{ weekString }}
    </h1>
    <div class="grid column spanning-4" v-for="pkg in packages" :key="pkg.name">
      <router-link
        :to="'/package/' + pkg.name"
        v-if="pkg.weekInfo && pkg.weekInfo[0]"
      >
        <div class="card">
          <h2>{{ pkg.name }}</h2>
          <div class="sections">
            <div class="stats">
              <h3>&nbsp;</h3>
              <p class="stat">Downloads</p>
              <p class="stat">Forks</p>
              <p class="stat">Issues</p>
              <p class="stat">Stars</p>
              <p class="stat">Watchers</p>
            </div>
            <div
              class="section"
              v-for="(stats, i) in [
                pkg.lastWeek.statsStartOfWeek,
                pkg.thisWeek.statsStartOfWeek
              ]"
              :key="i"
            >
              <h3>{{ i == 0 ? "Last Week" : "This Week" }}</h3>
              <p class="value">{{ stats.downloads }}</p>
              <p class="value">{{ stats.forks }}</p>
              <p class="value">{{ stats.issues }}</p>
              <p class="value">{{ stats.stars }}</p>
              <p class="value">{{ stats.watchers }}</p>
            </div>
          </div>
        </div>
      </router-link>
    </div>
  </section>
</template>

<script>
import GraphQL from "@/lib/graphql";
import { defineComponent } from "vue";

export default defineComponent({
  name: "PackagesHome",

  data: function() {
    return {
      packages: [],
      weekString: ""
    };
  },

  beforeMount: function() {
    const today = new Date();
    const startOfWeek = new Date(
      today.setDate(today.getDate() - today.getDay())
    );
    const endOfWeek = new Date(
      today.setDate(today.getDate() - today.getDay() + 6)
    );

    const startOfWeekString = startOfWeek.toLocaleString().split(",")[0];
    const endOfWeekString = endOfWeek.toLocaleString().split(",")[0];

    this.weekString = startOfWeekString + "-" + endOfWeekString;

    GraphQL.query(
      `query($today:DateTime!) {
        packages {
          multiple {
            items {
              name
              weekInfo(dayOfWeek:$today, prevWeeks:1, nextWeeks:0) {
                statsStartOfWeek { ...stats }
                statsEndOfWeek { ...stats }
              }
            }
          }
        }
    }
    
    fragment stats on PackageStats {
      downloads
      forks
      issues
      stars
      watchers
    }
    `,
      { today: new Date().toISOString() }
    )
      .then(json => json.packages.multiple.items)
      .then(packages => {
        const emptyStats = {
          downloads: -1,
          forks: -1,
          issues: -1,
          stars: -1,
          watchers: -1
        };

        const emptyWeek = {
          statsStartOfWeek: emptyStats,
          statsEndOfWeek: emptyStats
        };

        // Set the thisWeek and lastWeek properties
        for (const pkg of packages) {
          pkg.weekInfo = pkg.weekInfo || [];

          pkg.thisWeek = emptyWeek;
          pkg.lastWeek = emptyWeek;

          if (pkg.weekInfo.length == 1) pkg.thisWeek = pkg.weekInfo[0];
          else {
            pkg.thisWeek = pkg.weekInfo[1]; // It's in chronological order
            pkg.lastWeek = pkg.weekInfo[0];
          }
        }

        return packages;
      })
      .then(packages => (this.packages = packages));
  }
});
</script>

<style lang="scss" scoped>
@import "@/styles/global.scss";

h1 {
  text-align: center;
  margin-bottom: 25px;
}

h2 {
  display: flex;
  align-self: center;
  color: get-colour("text-on-dark", "medium");
}

h3 {
  display: flex;
  align-self: center;
  color: get-colour("text-on-dark", "low");
  margin: 0;
  margin-bottom: 1em;
}

.card {
  display: flex;
  flex-direction: column;
  background-color: get-colour("background", "1");
  width: 90%;
  min-width: 320px; // Good enough for most modern phones and above.
  border-radius: 10px;
  margin-bottom: 1.5em;

  &:hover {
    background-color: get-colour("background", "2");
  }
}

.sections {
  display: flex;
}

.section {
  display: flex;
  flex-direction: column;
  width: 35%;

  &:nth-child(2) {
    border-right: 4px solid get-colour("background", "8");
  }
}

.stats {
  display: flex;
  flex-direction: column;
  width: 30%;
  margin-left: 1em;
}

.stat {
  color: get-colour("text-on-dark", "high");
}

.value {
  display: flex;
  align-self: center;
  color: get-colour("accent", "700");
}
</style>
