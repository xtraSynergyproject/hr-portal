const fetch = require('node-fetch');
import {
  convertStringPropToFunction,
  transformDimensions,
  transformMeasures,
} from './utils';

asyncModule(async () => {
	const dynamicCubesString = await (
	await fetch('https://webapidev.aitalkx.com/common/query/GetSynergySchema')
	).text();
	//const dotenv = require('dotenv');
	//dotenv.config();
    //var url = dotenv.CUBEJS_SCHEMA_URL;
    var jsonParse = JSON.parse(dynamicCubesString);

	jsonParse.forEach((dynamicCubeS) => {
	console.log(dynamicCubeS);
	//var dimensionStr = "{\"Name\":{\"sql\":\"\\\"Name\\\"\",\"type\":\"string\"}}";
	var dimensionStr = "";
	var i =0;
	dynamicCubeS.dimensions.forEach((dimen) => {
		if(i!=0) dimensionStr = dimensionStr + ",";
		if(i==0) dimensionStr = dimensionStr + "{";
		dimensionStr = dimensionStr + "\""+dimen.name+"\":{\"sql\":\"\\\""+dimen.name+"\\\"\",\"type\":\"string\"}";
		i++;
	});
	dimensionStr = dimensionStr + "}";
	var dimJson = JSON.parse(dimensionStr);
	const dimensions = transformDimensions(dimJson);

   
	cube(dynamicCubeS.name, {
	sql: dynamicCubeS.sql,
	
	joins: {
		
	},
	
	measures: {
		count: {
		type: `count`,
		}
	},
	
	dimensions: dimensions,
	
	dataSource: `default`
	});
}); 

});
