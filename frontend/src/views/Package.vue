<template>
  <div class="grid container background-1">
    <div class="grid column spanning-12">
      <h1>Overview for package {{ name }}</h1>
    </div>
    <div class="grid container column spanning-12">
      <div id="content">
        <div>
          Showing last
          <input
            id="week-range"
            type="number"
            v-model="prevWeeks"
            @input="onWeekRangeChange"
          />
          weeks.
        </div>
        <canvas id="chart"></canvas>
      </div>
    </div>
  </div>
</template>

<script>
import Chart from "chart.js";
import * as lodash from "lodash";
import GraphQL from "@/lib/graphql";

export default {
  props: {
    name: {
      type: String
    }
  },

  data: function() {
    return {
      chart: null,
      prevWeeks: 1
    };
  },

  mounted: function() {
    this._createChart();
    this._updateChart();
  },

  methods: {
    onWeekRangeChange: lodash.debounce(function() {
      this._updateChart();
    }, 500),

    _createChart: function() {
      Chart.defaults.global.defaultFontColor = "rgba(255, 255, 255, 0.87)"; // Same as text-on-dark("high")
      this.chart = new Chart(
        document.getElementById("chart").getContext("2d"),
        {
          type: "line",
          data: {
            labels: [],
            datasets: [
              {
                label: "Downloads",
                backgroundColor: "#008688" // accent("800")
              },
              {
                label: "Forks",
                hidden: true,
                backgroundColor: "#398a00" // green shade of accent("800")
              },
              {
                label: "Issues",
                hidden: true,
                backgroundColor: "#8a0000" // red shade of accent("800")
              },
              {
                label: "Stars",
                hidden: true,
                backgroundColor: "#858a00" // yellow shade of accent("800")
              },
              {
                label: "Watchers",
                hidden: true,
                backgroundColor: "#63008a" // purple shade of accent("800")
              }
            ]
          },
          options: {
            scales: {
              xAxes: [
                {
                  type: "time",
                  time: {
                    unit: "week"
                  }
                }
              ],
              yAxes: [
                {
                  type: "linear",
                  ticks: {
                    beginAtZero: true
                  }
                }
              ]
            }
          }
        }
      );
    },

    _updateChart: function() {
      // TODO: Add a lock here.
      GraphQL.query(
        `query($name:String!, $today:DateTime!, $nextWeeks:UInt!, $prevWeeks:UInt!) {
          packages {
            single(name:$name) {
              weekInfo(dayOfWeek:$today, prevWeeks:$prevWeeks, nextWeeks:$nextWeeks) {
                week {
                  start
                  end
                }
                statsStartOfWeek { ...stats }
                statsEndOfWeek { ...stats }
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
      }`,
        {
          name: this.name,
          today: new Date().toISOString(),
          nextWeeks: 0,
          prevWeeks: this.prevWeeks
        }
      )
        .then(json => json.packages.single.weekInfo)
        .then(weeks => {
          const data = this.chart.data;
          data.labels = [];

          // Find the datasets.
          const datasets = {
            downloads: data.datasets.filter(ds => ds.label === "Downloads")[0],
            forks: data.datasets.filter(ds => ds.label === "Forks")[0],
            issues: data.datasets.filter(ds => ds.label === "Issues")[0],
            stars: data.datasets.filter(ds => ds.label === "Stars")[0],
            watchers: data.datasets.filter(ds => ds.label === "Watchers")[0]
          };

          // Clear their data, and pre-set the length to what we need.
          for (const dataset of Object.values(datasets)) {
            dataset.data = [];
            dataset.data.length = weeks.length;
          }

          // Add labels and the startOfWeek data for each week.
          for (const [index, week] of weeks.entries()) {
            const startDate = new Date(week.week.start);

            data.labels.push(startDate);
            datasets.downloads.data[index] = week.statsStartOfWeek.downloads;
            datasets.forks.data[index] = week.statsStartOfWeek.forks;
            datasets.issues.data[index] = week.statsStartOfWeek.issues;
            datasets.stars.data[index] = week.statsStartOfWeek.stars;
            datasets.watchers.data[index] = week.statsStartOfWeek.watchers;
          }

          this.chart.update();
        });
    }
  }
};
</script>

<style lang="scss" scoped>
@import "@/styles/global.scss";

#chart {
  width: 100%;
  height: auto;
  margin-top: 1em;
}

#content {
  margin: 1em;
}

#week-range {
  width: 40px;
}

h1 {
  margin: 1em;
  margin-top: 0.5em;
}

.background-1 {
  background-color: get-colour("background", "1");
}
</style>
