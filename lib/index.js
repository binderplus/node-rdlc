var edge 	= require('edge')
var fs   	= require('fs')
var path	= require('path')
var _		= {
	defaults: require('lodash.defaults')
}

var func = edge.func({
	source: path.join(__dirname, './index.cs'),
	references: [path.join(
		process.env['windir'],
		'assembly/GAC_MSIL/Microsoft.ReportViewer.WebForms/10.0.0.0__b03f5f7f11d50a3a', 
		'Microsoft.ReportViewer.WebForms.dll'
	)]
})

module.exports = function build (options, callback) {

	if (typeof callback !== 'function')
		throw new TypeError('callback should be a function')

	if (typeof options !== 'object')
		throw new TypeError('options should be a configuration object. See README.')

	if (!options.report || !fs.existsSync(options.report))
		throw new Error('can\'t find report file: '+options.report)

	// Convert parameters to String
	if (options.parameters) {
		for (param in options.parameters)
			if (typeof options.parameters[param] !== 'string')
				options.parameters[param] = String(options.parameters[param])
	}

	// Improve data API
	// TODO

	func(options, function (err, result) {

		// Recursively report InnerExceptions
		if (!!err) {
			var errMessage = "";
			do {
				if (!!err.Message) errMessage += (err.Message, err.Code?'('+err.Code+')':'')
				else errMessage += err;
				err = err.InnerException;
			} while (!!err)
			return callback(new Error(errMessage))
		}

		// Return result (buffer)
		callback(null, result)
	})

}