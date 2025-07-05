using global::CommandLine;
using KisaragiTech.Dape.CommandLine;
using System.IO;
using System.Threading.Tasks;
using KisaragiTech.Dape.Config;
using Neo4j.Driver;
using System;
using KisaragiTech.Dape.User.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace KisaragiTech.Dape;

internal static class Program
{
    static int Main(string[] args)
    {
        var sw = Parser.Default.ParseArguments<Options>(args).GetOrThrow()!;
        var configPath = sw.RunDir + "/config.json";
        if (!File.Exists(configPath))
        {
            Console.Error.WriteLine($"パス {configPath} が存在しません");
            return 1;
        }
        var config = RootConfig.DeserializeFromJson(File.ReadAllText(configPath));
        if (config == null)
        {
            throw new Exception("null");
        }

        if (config.Database == null)
        {
            throw new Exception("database is null");
        }

        var db = GraphDatabase.Driver($"bolt://{config.Database.Host}:{config.Database.Port}", AuthTokens.Basic(config.Database.User, config.Database.Password));
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            // TODO: Dockerを導入するときにランダムポートからコンフィグ指定にする
            serverOptions.Listen(System.Net.IPAddress.Loopback, 0);
        });
        
        var app = builder.Build();

        // 開発環境ではデベロッパーツールを使用
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // TODO: UseDeveloperExceptionPage は HTML でエラーを返すらしいので、実際のAPIの運用を始めるときはExceptionを
            // JSONで返せるようにしておいたほうが良さそう
        }

        app.Lifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                var repo = new UserRepositoryImpl(db);
                var isInitialized = await repo.HasRootUser();

                if (isInitialized)
                {
                    Console.WriteLine("Initialized");
                }
                else
                {
                    Console.WriteLine("Not initialized");
                    // TODO: ルートユーザー作成・挿入処理
                }
            });
        });
        // IPアドレスとポートは起動時に表示されるので不要

        // 静的ファイルとルーティングを有効化
        // app.UseStaticFiles();
        app.UseRouting();

        // ルートエンドポイントの定義
        app.MapGet("/init", () =>
        {
            // TODO
        });

        app.Run();

        return 0;
    }
}
