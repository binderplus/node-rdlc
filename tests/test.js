var rdlc = require('../index.js')


rdlc ({ 

	report: 'test.rdl', 

	parameters: {
		param1: 1,
		param2: 2,
		param3: 'Hello World!'
	},

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