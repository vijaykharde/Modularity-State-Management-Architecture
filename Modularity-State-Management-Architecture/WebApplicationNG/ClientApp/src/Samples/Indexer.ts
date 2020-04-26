export class foo {
    this: Dictionary;
    //get foo[index]=>any=function(p1):any{ return p1; };

    /*get(key:string):any{
      console.log(key);
    }*/
    /*set(key:string,value:any):void{
      console.log(key);
      console.log(value);
    }*/
    /*get : foo[key:string]=>function(key:string):any{
      return null;
    };*/
    /*set : (key:string,value:any)=>void=function(key,value):void{
        console.log(key);
    };*/

    /*get this():any{
      console.log(this);
      return this;
    };*/
}
interface IFoo {
  [key:string]: Dictionary;
}
type Dictionary={ [key:string]: any };