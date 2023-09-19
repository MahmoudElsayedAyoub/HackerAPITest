 GNU GENERAL PUBLIC LICENSE
 Version 2, June 1991

 Copyright (C) 1989, 1991 Free Software Foundation, Inc.,
 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.

 --
Our api is generate three Api nous 

ListBestStories : return all stories

GetBestStoriesByN : return best stories by give specif n of stories 

GetBestStoriesWithDeatilsByN : return  stories with deatils  by give specif n of stories 

GetBestStorieswithDeatilsV2ByN :  return  stories with deatils  by give specif n of stories  v2

GetStoriesById: get specific story by Id

--

We are using some LIbaray like

BenchmarkDotNet helps you to transform methods into benchmarks, track their performance, and share reproducible measurement experiments. 
It's no harder than writing unit tests! Under the hood, it performs a lot of magic that guarantees reliable and precise results thanks to the perfolizer statistical engine.
 BenchmarkDotNet protects you from popular benchmarking mistakes and warns you if something is wrong with your benchmark design or obtained measurements. 
 The results are presented in a user-friendly form that highlights all the important facts about your experiment.
 BenchmarkDotNet is already adopted by 17400+ GitHub projects including .NET Runtime, .NET Compiler, .NET Performance, and many others.


AND

Parallel.ForAsync


ForEachAsync<TSource>(IAsyncEnumerable<TSource>, Func<TSource,CancellationToken,ValueTask>)

