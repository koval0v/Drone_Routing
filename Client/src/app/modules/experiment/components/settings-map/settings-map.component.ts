import { Component, OnInit } from '@angular/core';
import { Settings } from 'src/app/models/Settings.model';
import { ChartOptionsColumn } from '../diagrams/columns/columns.component';
import { RangebarMapComponent } from '../diagrams/rangebar-map/rangebar-map.component';

@Component({
  selector: 'app-settings-map',
  templateUrl: './settings-map.component.html',
  styleUrls: ['./settings-map.component.css']
})
export class SettingsMapComponent implements OnInit {
  settings : Settings = new Settings();
  mapChartOptions: Partial<ChartOptionsColumn> | undefined;

  component: RangebarMapComponent | undefined;
  isValidForm: boolean = false;

  ngOnInit() {
    this.mapChartOptions = {
      series: [{
        name: 'Загальна територія',
        data: [
          {
            x: this.settings.General.xRange.startCoordinate,
            y: [this.settings.General.yRange.startCoordinate, this.settings.General.yRange.endCoordinate],
          },
          {
            x: this.settings.General.xRange.endCoordinate,
            y: [this.settings.General.yRange.startCoordinate, this.settings.General.yRange.endCoordinate]
          },
        ],
        color: "#E2E2E2",
        type: 'rangeArea',
      }, {
      name: 'Початкова база',
      data: [
        {
          x: this.settings.StartBase.xRange.startCoordinate,
          y: [this.settings.StartBase.yRange.startCoordinate, this.settings.StartBase.yRange.endCoordinate]
        },
        {
          x: this.settings.StartBase.xRange.endCoordinate,
          y: [this.settings.StartBase.yRange.startCoordinate, this.settings.StartBase.yRange.endCoordinate]
        },
      ],
      color: "#4746BD",
      type: 'rangeArea',
    }, {
      name: 'Кінцева база',
      data: [
        {
          x: this.settings.EndBase.xRange.startCoordinate,
          y: [this.settings.EndBase.yRange.startCoordinate, this.settings.EndBase.yRange.endCoordinate]
        },
        {
          x: this.settings.EndBase.xRange.endCoordinate,
          y: [this.settings.EndBase.yRange.startCoordinate, this.settings.EndBase.yRange.endCoordinate]
        },
      ],
      color: "#FF521D",
      type: 'rangeArea',
    }],
      chart: {
      height: 520,
      width: '95%',
      type: 'rangeArea',
      fontFamily: 'Harmonia Sans Pro Cyr',
      toolbar: {
        show: false,
      },
    },
    colors: ["#4746BD", "#4746BD"],
    plotOptions: {
      bar: {
        borderRadius: 0,
        dataLabels: {
          position: 'top',
        },
        barHeight: '90%',
        distributed: false,
      },
    },
    dataLabels: {
      enabled: false,
    },
    fill: {
      opacity: [1, 1, 1]
    },
    xaxis: {
      min: 0,
      max: this.settings.General.xRange.endCoordinate * 1.3,
      tickAmount: (this.settings.General.xRange.endCoordinate * 1.3) / 3,
      labels: {
        style: {
          fontSize: '14px',
          fontWeight: 500,
        },
      },
      crosshairs: {
        show: true,
        position: 'front',
        stroke: {
          color: '#4746BD',
          width: 1.3,
          dashArray: 4
        }
      }
    },
    yaxis: {
      min: 0,
      max: this.settings.General.yRange.endCoordinate * 1.3,
      tickAmount: (this.settings.General.yRange.endCoordinate * 1.3) / 6,
      labels: {
        style: {
          fontSize: '14px',
          fontWeight: 500,
        },
      },
      /*crosshairs: {
        show: true,
        position: 'front',
        stroke: {
          color: '#4746BD',
          width: 2,
          dashArray: 5
        }
      }*/
    },
    grid: {
      borderColor: '#BDBDBD',
    },
    legend: {
      position: 'bottom',
      fontSize: '14px',
      fontWeight: 500,
      itemMargin: {
        horizontal: 15,
        vertical: 5,
      },
      offsetX: 0,
      offsetY: 0,
    },
    tooltip: {
      enabled: true
    },
    markers: {},
    title: {
      text: "Території генерування об'єктів",
      offsetY: 0,
      align: 'center',
      style: {
        fontSize: '24px',
        fontWeight: '800',
        color: '#324068'
      },
        margin: 30,
      }
    };
  }

  changeMapChart(settings: Settings, isValidForm: boolean | null ) {
    if (isValidForm == null) {
      this.isValidForm = false;
    } else {
      this.isValidForm = isValidForm;
    }
    this.mapChartOptions = {...this.mapChartOptions, ...{
      yaxis: {
        max: settings.General.yRange.endCoordinate + 10,
        tickAmount: (settings.General.yRange.endCoordinate + 10) >= 100 ?
          Math.round((settings.General.yRange.endCoordinate + 10) / 40) :
          Math.round((settings.General.yRange.endCoordinate + 10) / 4)
      },
      xaxis: {
        max: settings.General.xRange.endCoordinate + 10,
        tickAmount: (settings.General.xRange.endCoordinate + 10) >= 100 ?
        Math.round((settings.General.xRange.endCoordinate + 10) / 40) :
        Math.round((settings.General.xRange.endCoordinate + 10) / 4)
    }}};

    this.mapChartOptions!.series = [{
        name: 'Загальна територія',
        data: [
          {
            x: settings.General.xRange.startCoordinate,
            y: [settings.General.yRange.startCoordinate, settings.General.yRange.endCoordinate],
          },
          {
            x: settings.General.xRange.endCoordinate,
            y: [settings.General.yRange.startCoordinate, settings.General.yRange.endCoordinate]
          },
        ],
        color: "#E2E2E2",
      }, {
      name: 'Початкова база',
      data: [
        {
          x: settings.StartBase.xRange.startCoordinate,
          y: [settings.StartBase.yRange.startCoordinate, settings.StartBase.yRange.endCoordinate]
        },
        {
          x: settings.StartBase.xRange.endCoordinate,
          y: [settings.StartBase.yRange.startCoordinate, settings.StartBase.yRange.endCoordinate]
        },
      ],
      color: "#4746BD",
    }, {
      name: 'Кінцева база',
      data: [
        {
          x: settings.EndBase.xRange.startCoordinate,
          y: [settings.EndBase.yRange.startCoordinate, settings.EndBase.yRange.endCoordinate]
        },
        {
          x: settings.EndBase.xRange.endCoordinate,
          y: [settings.EndBase.yRange.startCoordinate, settings.EndBase.yRange.endCoordinate]
        },
      ],
      color: "#FF521D",
    }];
  }
}
