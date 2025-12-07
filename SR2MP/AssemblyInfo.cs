using System.Reflection;
using MelonLoader;
using SR2E.Expansion;
using Main = SR2MP.Main;

[assembly: MelonInfo(typeof(Main), "SR2MP", "1.0.0", "Shark")]
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]

[assembly: AssemblyTitle(Main.BuildInfo.Name)]
[assembly: AssemblyDescription(Main.BuildInfo.Description)]
[assembly: AssemblyCompany(Main.BuildInfo.Company)]
[assembly: AssemblyProduct(Main.BuildInfo.Name)]
[assembly: AssemblyCopyright($"Created by {Main.BuildInfo.Author}")]
[assembly: AssemblyTrademark(Main.BuildInfo.Company)]
[assembly: VerifyLoaderVersion(0,6,2, true)]
[assembly: AssemblyVersion(Main.BuildInfo.Version)]
[assembly: MelonPriority(-100)]
[assembly: AssemblyFileVersion(Main.BuildInfo.Version)]
[assembly: MelonInfo(typeof(Main), Main.BuildInfo.Name, Main.BuildInfo.Version, Main.BuildInfo.Author, Main.BuildInfo.DownloadLink)]
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]
[assembly: MelonColor(255, 35, 255, 35)]
[assembly: MelonAdditionalDependencies("SR2E")]
[assembly: AssemblyMetadata("co_authors",Main.BuildInfo.CoAuthors)]
[assembly: AssemblyMetadata("contributors",Main.BuildInfo.Contributors)]
[assembly: AssemblyMetadata("source_code",Main.BuildInfo.SourceCode)]
[assembly: AssemblyMetadata("nexus",Main.BuildInfo.Nexus)]
[assembly: AssemblyMetadata("discord","https://discord.com/invite/a7wfBw5feU")]
[assembly: SR2EExpansion(Main.BuildInfo.UsePrism)]