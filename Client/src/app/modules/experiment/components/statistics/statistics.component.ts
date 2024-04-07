import { Component, OnChanges, OnInit } from '@angular/core';
import { ExperimentStatistics } from 'src/app/models/ExperimentStatistics.model';
import { AlgorithmTypes } from 'src/app/models/enums/AlgorithmTypes.model';
import { ChartOptions } from '../diagrams/pie-chart/pie-chart.component';
import { TheNearestNeighbourTypes } from 'src/app/models/enums/TheNearestNeighbourTypes.model';
import { TheNearestToTheLineTypes } from 'src/app/models/enums/TheNearestToTheLineTypes.model';
import { AlgorithmDeviations } from 'src/app/models/AlgorithmDeviations.model';
import { ChartOptionsColumn } from '../diagrams/columns/columns.component';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  statistics : ExperimentStatistics = {} as ExperimentStatistics;
  maxDistancePercent : number = 0;
  maxTargetsPercent : number = 0;
  deviations : AlgorithmDeviations | undefined;
  squareDeviations : AlgorithmDeviations | undefined;
  theBestOnlyTargets : number | undefined;
  theBestTargetsDistance : number | undefined;
  theWorstOnlyTargets : number | undefined;
  theWorstTargetsDistance : number | undefined;

  type : AlgorithmTypes | undefined;

  targetsPieOptions: Partial<ChartOptions> | undefined;
  targetsDistancePieOptions: Partial<ChartOptions> | undefined;
  columnOptionsExecutionTime: Partial<ChartOptionsColumn> | undefined;
  columnOptionsTargetsCount: Partial<ChartOptionsColumn> | undefined;
  columnOptionsDistance: Partial<ChartOptionsColumn> | undefined;
  columnOptionsTargetsDynamic: Partial<ChartOptionsColumn> | undefined;

  ngOnInit(): void {
    this.maxDistancePercent = 100 * Math.round(this.statistics.averageDistance) / (this.statistics.averageSpeed * this.statistics.timeResource)
    this.maxTargetsPercent = 100 * Math.round(this.statistics.averageTargetsCount) / this.statistics.targetsCount;

    console.log(this.statistics);

    this.targetsPieOptions = {
      series: [this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
          === TheNearestNeighbourTypes.Standard.toString() && i.theBestByTargets).length | 0,
        this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
          === TheNearestNeighbourTypes.InTheArea.toString() && i.theBestByTargets).length | 0,
        this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
          === TheNearestNeighbourTypes.Parallel.toString() && i.theBestByTargets).length | 0,
        this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
          === TheNearestToTheLineTypes.OrderingByX.toString() && i.theBestByTargets).length | 0,
        this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
          === TheNearestToTheLineTypes.OrderingByDistances.toString() && i.theBestByTargets).length | 0],
      colors: ["#E93A31", "#272588", "#FFBB00", "#FF521D", "#4746BD"],
      stroke: {
        show: false,
      },
      chart: {
        width: '105%',
        type: 'pie',
        offsetX: -5,
        offsetY: 0,
        fontFamily: 'Harmonia Sans Pro Cyr',
      },
      dataLabels: {
        style: {
          fontSize: '12px',
        },
        dropShadow: {
          enabled: false,
        },
      },
      legend: {
        position: 'right',
        fontSize: '14px',
        fontWeight: 500,
        itemMargin: {
          vertical: 5,
        },
        offsetX: 0,
        offsetY: 30,
      },
      title: {
        text: "Успіх алгоритму за кількістю об'єктів, разів",
        align: 'center',
        style: {
          fontSize:  '16px',
          fontWeight:  '500',
          color:  '#324068'
        },
        margin: 20,
      },
      labels: ["Найближчий сусід", "Найближчий сусід (ділянка)", "Найближчий сусід (паралельно)",
        "Найближча точка (X)", "Найближча точка (матриця)"],
      responsive: [{
        breakpoint: 480,
        options: {
          chart: {
            width: 200,
          },
        }
      },]
    };

    this.targetsDistancePieOptions = JSON.parse(JSON.stringify(this.targetsPieOptions));
    this.targetsDistancePieOptions!.series = [this.statistics.totalResults.flatMap(x => x.algorithmsResults)
      .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
      === TheNearestNeighbourTypes.Standard.toString() && i.theBestByTargetsAndDistance).length | 0,
    this.statistics.totalResults.flatMap(x => x.algorithmsResults)
      .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
      === TheNearestNeighbourTypes.InTheArea.toString() && i.theBestByTargetsAndDistance).length | 0,
    this.statistics.totalResults.flatMap(x => x.algorithmsResults)
      .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
      === TheNearestNeighbourTypes.Parallel.toString() && i.theBestByTargetsAndDistance).length | 0,
    this.statistics.totalResults.flatMap(x => x.algorithmsResults)
      .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
      === TheNearestToTheLineTypes.OrderingByX.toString() && i.theBestByTargetsAndDistance).length | 0,
    this.statistics.totalResults.flatMap(x => x.algorithmsResults)
      .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
      === TheNearestToTheLineTypes.OrderingByDistances.toString() && i.theBestByTargetsAndDistance).length | 0];
    this.targetsDistancePieOptions!.title!.text = "Успіх алгоритму за кількістю об'єктів та відстанню, разів";

    /*const diagramColumns = document.querySelectorAll(`[id^="SvgjsPath"]`);
    let columnsValues: number[] = [];
    diagramColumns.forEach(x => {
      console.log(x.getAttribute("val"))
    });
    console.log("aaa")
    console.log(columnsValues.sort()[0])
    console.log(columnsValues.sort()[columnsValues.length - 1])*/
  }

  changeSection(algorithmType: AlgorithmTypes | undefined){
    this.type = algorithmType;
    if (algorithmType != undefined)
    {
      if (Object.values(TheNearestNeighbourTypes).includes(algorithmType as TheNearestNeighbourTypes))
      {
        this.deviations = this.statistics.deviations.find(x => !x.lineType &&
          Object.values(TheNearestNeighbourTypes)[+x.neighbourType.toString()] == algorithmType);
        this.squareDeviations = this.statistics.squareDeviations.find(x => !x.lineType &&
          Object.values(TheNearestNeighbourTypes)[+x.neighbourType.toString()] == algorithmType);

        this.theBestOnlyTargets = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
          === algorithmType && i.theBestByTargets).length;
        this.theBestTargetsDistance = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
          === algorithmType && i.theBestByTargetsAndDistance).length;

        this.theWorstOnlyTargets = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
          === algorithmType && i.theWorstByTargets).length;
        this.theWorstTargetsDistance = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.neighbourType != undefined && Object.values(TheNearestNeighbourTypes)[+i.neighbourType.toString()]
          === algorithmType && i.theWorstByTargetsAndDistance).length;
      }
      else
      {
        this.deviations = this.statistics.deviations.find(x => x.lineType != null &&
          Object.values(TheNearestToTheLineTypes)[+x.lineType.toString()] == algorithmType);
        this.squareDeviations = this.statistics.squareDeviations.find(x => x.lineType != null &&
          Object.values(TheNearestToTheLineTypes)[+x.lineType.toString()] == algorithmType);

        this.theBestOnlyTargets = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
          === algorithmType && i.theBestByTargets).length;
        this.theBestTargetsDistance = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
          === algorithmType && i.theBestByTargetsAndDistance).length;

        this.theWorstOnlyTargets = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
          === algorithmType && i.theWorstByTargets).length;
        this.theWorstTargetsDistance = this.statistics.totalResults.flatMap(x => x.algorithmsResults)
          .filter(i => i.lineType != undefined && Object.values(TheNearestToTheLineTypes)[+i.lineType.toString()]
          === algorithmType && i.theWorstByTargetsAndDistance).length;
      }
    }

    /*console.log(this.statistics)*/

    let statisticsData = this.statistics.totalResults.flatMap(x => x.algorithmsResults);
    let statisticsDataChart : number[] = [];
    let dynamicDataChart : number[] = [];

    if (Object.values(TheNearestNeighbourTypes).includes(algorithmType as TheNearestNeighbourTypes))
    {
      statisticsDataChart = statisticsData.filter(x => x.neighbourType != null &&
        Object.values(TheNearestNeighbourTypes)[+x.neighbourType!.toString()] == algorithmType).map(y => y.executionTime);
      dynamicDataChart = this.statistics.dynamicResults.filter(x => x.neighbourType != null &&
        Object.values(TheNearestNeighbourTypes)[+x.neighbourType!.toString()] == algorithmType).map(y => y.routeTargetsCount);
    }
    else {
      statisticsDataChart = statisticsData.filter(x => x.lineType != null &&
        Object.values(TheNearestToTheLineTypes)[+x.lineType.toString()] == algorithmType).map(y => y.executionTime);
      dynamicDataChart = this.statistics.dynamicResults.filter(x => x.lineType != null &&
        Object.values(TheNearestToTheLineTypes)[+x.lineType.toString()] == algorithmType).map(y => y.routeTargetsCount);
    }

    this.columnOptionsExecutionTime = {
      series: [{
      name: 'Час вирішення',
      data: statisticsDataChart,
      color: "#272588",
    }],
      chart: {
      height: 300,
      width: '98%',
      type: 'bar',
      fontFamily: 'Harmonia Sans Pro Cyr',
      toolbar: {
        show: false,
      }
    },
    colors: [
    ],
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
    xaxis: {
      labels: {
        style: {
          fontSize: '14px',
          fontWeight: 500,
        },
      },
    },
    yaxis: {
      labels: {
        style: {
          fontSize: '14px',
          fontWeight: 500,
        },
      },
    },
    grid: {
      borderColor: '#BDBDBD',
    },
    title: {
      text: 'Час вирішення задачі кожної задачі, мс',
      floating: true,
      offsetY: 0,
      align: 'center',
      style: {
        fontSize:  '16px',
        fontWeight:  '500',
        color:  '#324068'
      },
      margin: 30,
      }
    };

    if (Object.values(TheNearestNeighbourTypes).includes(algorithmType as TheNearestNeighbourTypes))
    {
      /*console.log(TheNearestNeighbourTypes);*/
      statisticsDataChart = statisticsData.filter(x => x.neighbourType != null &&
        Object.values(TheNearestNeighbourTypes)[+x.neighbourType!.toString()] == algorithmType).map(y => y.route.length - 2);
    }
    else {
      /*console.log(TheNearestToTheLineTypes);*/
      statisticsDataChart = statisticsData.filter(x => x.lineType != null &&
          Object.values(TheNearestToTheLineTypes)[+x.lineType.toString()] == algorithmType).map(y => y.route.length - 2);
    }

    this.columnOptionsTargetsCount = JSON.parse(JSON.stringify(this.columnOptionsExecutionTime));
    this.columnOptionsTargetsCount!.title!.text = "Кількість об'єктів маршруту кожної задачі";
    this.columnOptionsTargetsCount!.series![0].data = statisticsDataChart;
    this.columnOptionsTargetsCount!.series![0].name = "Кількість об'єктів";

    if (Object.values(TheNearestNeighbourTypes).includes(algorithmType as TheNearestNeighbourTypes))
    {
      /*console.log(TheNearestNeighbourTypes);*/
      statisticsDataChart = statisticsData.filter(x => x.neighbourType != null &&
        Object.values(TheNearestNeighbourTypes)[+x.neighbourType!.toString()] == algorithmType).map(y => Math.trunc(y.routeDistance));
    }
    else {
      /*console.log(TheNearestToTheLineTypes);*/
      statisticsDataChart = statisticsData.filter(x => x.lineType != null &&
          Object.values(TheNearestToTheLineTypes)[+x.lineType.toString()] == algorithmType).map(y => Math.trunc(y.routeDistance));
    }

    this.columnOptionsDistance = JSON.parse(JSON.stringify(this.columnOptionsExecutionTime));
    this.columnOptionsDistance!.title!.text = "Довжина маршруту кожної задачі, км";
    this.columnOptionsDistance!.series![0].data = statisticsDataChart;
    this.columnOptionsDistance!.series![0].name = "Довжина маршруту";

    this.columnOptionsTargetsDynamic = JSON.parse(JSON.stringify(this.columnOptionsExecutionTime));
    this.columnOptionsTargetsDynamic!.title!.text = "Ефективність алгоритму залежно від кількості об'єктів на мапі";
    this.columnOptionsTargetsDynamic!.series![0].data = dynamicDataChart;
    this.columnOptionsTargetsDynamic!.series![0].name = "Кількість об'єктів побудованого маршруту";
  }

  ngAfterContentChecked() {
    if (this.type != undefined) {
      this.colorDiagramColumns();
    }
  }

  colorDiagramColumns(diagramsQuantity: number = 4) {
    let chartsOptions: Partial<ChartOptionsColumn>[] = [];
    if (this.columnOptionsExecutionTime != undefined && this.columnOptionsTargetsCount != undefined
        && this.columnOptionsDistance != undefined && this.columnOptionsTargetsDynamic != undefined) {
      chartsOptions = [this.columnOptionsExecutionTime, this.columnOptionsTargetsCount,
        this.columnOptionsDistance, this.columnOptionsTargetsDynamic];
    }
    for (let i = 0; i < diagramsQuantity; i++) {
      const diagramColumns = document.getElementsByClassName("apexcharts-series")[i].querySelectorAll(`[id^="SvgjsPath"]`);
      diagramColumns.forEach(x => {
        let minValue, avgValue, maxValue: string | undefined;
        if (chartsOptions != undefined && chartsOptions[i] != undefined) {
          minValue = this.sortNumbers(chartsOptions[i].series![0].data as number[])[0].toString();
          avgValue = this.sortNumbers(chartsOptions[i].series![0].data as number[])
            [Math.round((chartsOptions[i].series![0].data.length - 1) / 2)].toString();
          maxValue = this.sortNumbers(chartsOptions[i].series![0].data as number[])
            [chartsOptions[i].series![0].data.length - 1].toString();
        }
        if (minValue != undefined && x.getAttribute("val") == minValue) {
          x.setAttribute("fill", "rgba(255, 187, 0, 1)");
        }
        if (avgValue != undefined && x.getAttribute("val") == avgValue) {
          x.setAttribute("fill", "rgba(255, 82, 29, 1)");
        }
        if (maxValue != undefined && x.getAttribute("val") == maxValue) {
          x.setAttribute("fill", "rgba(233, 58, 49, 1)");
        }
      });
    }
  }

  sortNumbers(numbers: number[]): number[] {
    return (JSON.parse(JSON.stringify(numbers)) as number[]).sort((a, b) => a - b);
}
}
