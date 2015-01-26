# node-rdlc
Node.JS bindings to .NET's ReportViewer (Windows Only)

With this module you can run RDLC reports and populate them with javascript data. It's highly recomended that you read [Microsoft's Report Documentation](https://msdn.microsoft.com/en-us/library/bb885185.aspx) and that you use [Report Designer](https://msdn.microsoft.com/en-us/library/bb558708.aspx) to build your report designs (RDLC files)

There's a lot of documentation on the web on how to create usefull report designs. This documentation will only handle specifics on how to run and pass data to a report from Node.JS, but will assume you know how to design a report.

### Populate Data

Basically, there are two ways to pass data: Report parameters (variables) and Report Datasets (tables).

#### Report Parameters

Populate report parameters by passing setting a parameters object in the configuration object. Each key is a parameter.

```js
var rdlc = require('../index.js')

rdlc ({ 

	report: 'test.rdl', 

	parameters: {
		param1: 1,
		param2: 2,
		param3: 'Hello World!'
	}

}, function (err, result) {
	if (!!err) throw err;
	var fs = require('fs')
	fs.writeFileSync('test.pdf', result)
})
```

Parameters act like variables or constants inside the report. They represent a single value.

NOTE: You have to make sure the RDLC Report is expecting the parameters you send.

#### Report DataSets

Report datasets are tables accessible inside the report.
You should take a look at the examples since they are easily understandable.

TODO: Improve this documentation

```js
var rdlc = require('../index.js')

rdlc ({ 

	report: 'test.rdl', 

	data: {

		DataSet1: [
			{ name: 'Barry Allen', id: 1 },
			{ name: 'Oliver Queen', id: 2 },
			{ name: 'Clark Kent', id: 3 }
		]

	}

}, function (err, result) {
	if (!!err) throw err;
	var fs = require('fs')
	fs.writeFileSync('test.pdf', result)
})
```

NOTE: The report must be properly designed to expect the corresponding data format.
