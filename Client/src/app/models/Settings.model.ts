import { Coordinates } from "./Coordinates.model";
import { Range } from "./Range.model";

export class Settings {
    tasksQuantity : number = 0;
    targetsQuantity : number = 0;
    timeResource : number = 0;
    averageSpeed : number = 0;
    distributionIsUniform : boolean = false;
    General : Coordinates = new Coordinates(new Range(0, 0), new Range(0, 0));
    StartBase : Coordinates = new Coordinates(new Range(0, 0), new Range(0, 0));
    EndBase : Coordinates = new Coordinates(new Range(0, 0), new Range(0, 0));
}
