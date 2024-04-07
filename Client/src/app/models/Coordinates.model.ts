import { Range } from "./Range.model";

export class Coordinates {
  xRange : Range = new Range(0, 0);
  yRange : Range = new Range(0, 0);

  constructor(xRange: Range, yRange: Range) {
    this.xRange = xRange;
    this.yRange = yRange;
  }
}
