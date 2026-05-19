import { MapRule } from "../types";
import { LoginModel, RegisterModel } from "./generated";

export interface ResponseList<T> {
    total: number,
    data: T[]
}

const RegisterRule: MapRule = {
    method: 'createRegister',
    map: (input:any) => ({registerModel:input as RegisterModel})
}

const updateAuthRule: MapRule = {
    method: 'update',
    map: (input:any) => ({registerModel:input as RegisterModel})
}

const rules : MapRule[] = [
    RegisterRule
] ;

export const map = (method:string, input: any) => {
    method = method.replace("bound ", "");
   var rule = rules.find(x=>x.method == method);

    if (!rule)
        return input;

   return rule.map(input);
} 