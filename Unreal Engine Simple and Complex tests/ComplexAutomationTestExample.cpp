#include "Tests/AutomationEditorCommon.h"// required for FAutomationEditorCommonUtils along with, i believe, the "UnrealEd" module
#include "FileHelpers.h" // believe this requires the UnrealEd module
#include "Tests/AutomationCommon.h"
#include "EngineAnalytics.h"
#include "Kismet/GameplayStatics.h"
#include "Enemy.h"

IMPLEMENT_COMPLEX_AUTOMATION_TEST(LoadAllMapsInEditorTest, "AutomationTestingExample.SimpleAndComplexAutomationTests.Complex.AllMaps.LoadAllMaps", EAutomationTestFlags::EditorContext |EAutomationTestFlags::StressFilter)
IMPLEMENT_COMPLEX_AUTOMATION_TEST(ComplexEnemiesMaxHealthTest, "AutomationTestingExample.SimpleAndComplexAutomationTests.Complex.AllEnemies.HealthComponent.MaxHealth", EAutomationTestFlags::EditorContext | EAutomationTestFlags::StressFilter)

//test to loadall maps without crashing 
//https://github.com/EpicGames/UnrealEngine/blob/16dc333db3d6439c7f2886cf89db8907846c0e8a/Engine/Plugins/Tests/EditorTests/Source/EditorTests/Private/UnrealEd/EditorAutomationTests.cpp#L126

void LoadAllMapsInEditorTest::GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const
{
	TArray<FString> FileList;
	FEditorFileUtils::FindAllPackageFiles(FileList);

	//iterateover all files adding the ones with the map extension
	for (int FileIndex = 0; FileIndex < FileList.Num(); FileIndex++)
	{
		const FString& Filename = FileList[FileIndex];
		//Disregardfilenames that don't have the map extension if we're in MAPSONLY mode
		if (FPaths::GetExtension(Filename, true) == FPackageName::GetMapPackageExtension())
		{
			if (FAutomationTestFramework::Get().ShouldTestContent(Filename))
			{
				if (!Filename.Contains(TEXT("/Engine/")))
				{
					OutBeautifiedNames.Add(FPaths::GetBaseFilename(Filename));
					OutTestCommands.Add(Filename);
				}
			}
		}
	}
}
/**
 * Execute the loading of each map
 *
 * @param Parameters - Should specify which map name to load
 * @return	TRUE if the test was successful, FALSE otherwise
 */
bool LoadAllMapsInEditorTest::RunTest(const FString& Parameters)
{
	FString MapName = Parameters;
	double MapLoadStartTime = 0;
	//Test event for analytics. This should fire anytime this automation procedure is started.

	if (FEngineAnalytics::IsAvailable())
	{
		FEngineAnalytics::GetProvider().RecordEvent(TEXT("Editor.Usage.Test.Event"));
		UE_LOG(LogEditorAutomationTests, Log, TEXT("AnayticsTest: Load All Maps automation triggered and Editor.Usage.TestEvent analytic event has been fired."));
	}
	//Find the main editor window
	TArray<TSharedRef<SWindow>> AllWindows;
	FSlateApplication::Get().GetAllVisibleWindowsOrdered(AllWindows);
	if (AllWindows.Num() == 0)
	{
		UE_LOG(LogEditorAutomationTests, Error, TEXT("ERROR: Could not find the main editor window."));
		return false;
	}
	WindowScreenshotParameters WindowParameters;
	WindowParameters.CurrentWindow = AllWindows[0];

	//Disable Eye Adaptation
	static IConsoleVariable* CVar = IConsoleManager::Get().FindConsoleVariable(TEXT("r.EyeAdaptationQuality"));
	CVar->Set(0);

	//Create a screen shot filename and path
	const FString LoadAllMapsTestName = FString::Printf(TEXT("LoadAllMaps_Editor/%s"), *FPaths::GetBaseFilename(MapName));
	WindowParameters.ScreenshotName = AutomationCommon::GetScreenshotName(LoadAllMapsTestName);

	//Get the curent number of seconds. this will be used to track howlongit took to load the map.
	MapLoadStartTime = FPlatformTime::Seconds();
	//load the map
	FAutomationEditorCommonUtils::LoadMap(MapName);
	//log how long it took to launch the map
	UE_LOG(LogEditorAutomationTests, Display, TEXT("Map '%s' took %.3f to load"), *MapName, FPlatformTime::Seconds() - MapLoadStartTime);

	//if we dont have notexture streaming enabled, give the textures sometime to load.
	if (!FParse::Param(FCommandLine::Get(), TEXT("NoTextureStreaming")))
	{
		//givecontents some timeto load
		ADD_LATENT_AUTOMATION_COMMAND(FWaitLatentCommand(1.5f));
	}
	return true;
}


void ComplexEnemiesMaxHealthTest::GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const
{
	FString MapPath = FPaths::ProjectContentDir() / TEXT("Maps/FPSMap.umap");
	FAutomationEditorCommonUtils::LoadMap(MapPath);
	if (GEngine)
	{
		TArray<AActor*> actorArray;
		UWorld* world = GEngine->GetWorldContexts()[0].World();
		UGameplayStatics::GetAllActorsOfClass(world, AEnemy::StaticClass(), actorArray);
		if (!actorArray.IsEmpty())
		{
			for (int i = 0; i < actorArray.Num(); i++)
			{
				OutBeautifiedNames.Add(actorArray[i]->GetName());
				OutTestCommands.Add(actorArray[i]->GetName());

			}
		}
	}
}

bool ComplexEnemiesMaxHealthTest::RunTest(const FString& Parameters)
{
	FString MapPath = FPaths::ProjectContentDir() / TEXT("Maps/FPSMap.umap");
	AutomationOpenMap(MapPath);
	if (GEngine)
	{
		TArray<AActor*> actorArray;
		UWorld* world = GEngine->GetWorldContexts()[0].World();
		UGameplayStatics::GetAllActorsOfClass(world, AEnemy::StaticClass(), actorArray);
		if (!actorArray.IsEmpty())
		{
			for (int i = 0; i < actorArray.Num(); i++)
			{
				if (actorArray[i]->GetName() == Parameters)
				{
					AEnemy* enemy = Cast<AEnemy>(actorArray[i]);
					switch (enemy->getEnemyType())
					{
					case WEAK:
						TestEqual("Weak Enemy Health is Set to 100", enemy->health, 100);
						break;
					case STRONG:
						TestEqual("Strong Enemy Health is Set to 150", enemy->health, 150);
						break;
					case LEGENDARY:
						TestEqual("Legendary Enemy Health is Set to 200", enemy->health, 200);
						break;
					};
				}

			}
		}
	}
	return true;
}
