using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using RingoTabetterApi.POCOs;
using RingoTabetterApi.Models;
using Dapper;
using Dapper.FluentMap;
using RingoTabetterApi.FluentMaps;
using RingoTabetterTask.Helpers;

namespace RingoTabetterTask
{
    class Program
    {
        private const string Filter = @"^\[�����S\]";


        static void Main(string[] args)
        {
            // DB��Model�̃}�b�s���O
            FluentMapper.Intialize(config =>
            {
                var type = typeof(Apple);
                config.AddConvention<PropertyTransformConvention>().ForEntitiesInAssembly(type.Assembly, type.Namespace);
                config.AddMap(new LastSearchesMap());
            });

            var twitterHelper = new TwitterHelper();
            var tweets = twitterHelper.GatherTweets(Filter);

            var count = ModelHelper.RegisterWithTransaction(tweets);
            Console.WriteLine("���s�����F{0}�A�o�^�����F{1}��", DateTime.Now, count);
        }
    }
}
