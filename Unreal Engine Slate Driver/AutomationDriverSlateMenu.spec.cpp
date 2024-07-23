// This code is copyrighted by me :)
#include "Developer/AutomationDriver/Public/AutomationDriverCommon.h"
#include "Tests/AutomationEditorCommon.h"
#include <Tests/AutomationCommon.h>
#include "Kismet/GameplayStatics.h"

BEGIN_DEFINE_SPEC(AutomationDriverSpecTest, "AutomationDriver.SlateMenu.SpecTest", EAutomationTestFlags::ProductFilter | EAutomationTestFlags::ApplicationContextMask)
FAutomationDriverPtr Driver;
END_DEFINE_SPEC(AutomationDriverSpecTest)


void AutomationDriverSpecTest::Define()
{
    xBeforeEach([this]() {
        IAutomationDriverModule::Get().Enable();

        Driver = IAutomationDriverModule::Get().CreateDriver();
        UE_LOG(LogTemp, Warning, TEXT("BeforeEach"));
        AutomationOpenMap("/Game/Maps/MainMenu.MainMenu");
    });

    Describe("Slate Main Menu Driver Play Button", [this]() {
        
        Describe("Setup", [this]() {
            BeforeEach([this]() {
                AutomationOpenMap("/Game/Maps/MainMenu.MainMenu");
                });
            It("Should load the Main Menu and enable the driver", [this]() { //currently tryingtoadd before to load map thatwill runonce
                UWorld* world = nullptr;
                if (GEngine) world = GEngine->GameViewport->GetWorld();
                if (world)
                {
                    FString mapname = UGameplayStatics::GetCurrentLevelName(world, true);//world->GetMapName();
                    TestEqual("Main Menu is Loaded", mapname, "MainMenu");
                }

                IAutomationDriverModule::Get().Enable();
                Driver = IAutomationDriverModule::Get().CreateDriver();
                TestEqual("Driver is enabled and valid", Driver.IsValid(), true);
            });
        });
        

        It("Should Find and click the play button", EAsyncExecution::ThreadPool, [this]() {
            //FDriverElementRef TestBtn = Driver->FindElement(By::Id("TestButton")); //working with slate requires adding the slate HUD to gamemode defaults and removing umg from level bp
            FDriverElementRef TestBtn = Driver->FindElement(By::Path("#TestButton"));
            if (TestBtn->Exists())
            {
                TestBtn->Click();
                TestEqual("Button is interactable", TestBtn->IsInteractable(), true);
            }
            else
            {
                AddError("Error, Button doesn't exist");
            }
        });

        LatentIt("Wait for the game Map To Load", [this](const FDoneDelegate& Done) {
            UWorld* world = nullptr;
            if (GEngine) world = GEngine->GameViewport->GetWorld();
            if (world)
            {
                FString mapname = UGameplayStatics::GetCurrentLevelName(world, true);//world->GetMapName();
                UE_LOG(LogTemp, Warning, TEXT("MapName: %s"), *mapname);
                if (mapname == "FPSMap")
                {
                    TestEqual("Correct Game Map loaded", mapname, "FPSMap");
                    Done.Execute();
                }
            }
        });

        xIt("Should Load the next map", [this]() { //EAsyncExecution::ThreadPool,
            UWorld* world = nullptr;
            if (GEngine) world = GEngine->GameViewport->GetWorld();
            if (world)
            {
                FString mapname = world->GetMapName();
                TestEqual("Correct Map was loaded", mapname, "FPSMap");
            }
        });

    });

    xAfterEach([this]() {
        IAutomationDriverModule::Get().Disable();
        UE_LOG(LogTemp, Warning, TEXT("AfterEach"));
    });
}