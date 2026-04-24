
using Microsoft.EntityFrameworkCore;

var connString = @"Server=(localdb)\MSSQLLocalDB;Database=AppContext-dev;Trusted_Connection=True;MultipleActiveResultSets=true";
var outputPath = @"A:\Projects\App\Tools\Dictionary";

var options = new DbContextOptionsBuilder<DictionaryContext>()
    .UseSqlServer(connString)
    .Options;

var context = new DictionaryContext(options);

var dictionaries = context.Dictionaries.ToList();

DictionaryJsonGenerator.Generate(dictionaries, outputPath, (text, lang) => $"{text} [{lang}]");


