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
    Describe("Slate Main Menu Driver Play Button", [this]() {

        Describe("Setup Driver and find Play Button", [this]() {

            BeforeEach([this]() {
                AutomationOpenMap("/Game/Maps/MainMenu.MainMenu");
                IAutomationDriverModule::Get().Enable();
                Driver = IAutomationDriverModule::Get().CreateDriver();
                });

            It("Should Find and click the play button", EAsyncExecution::ThreadPool, [this]() {
                //FDriverElementRef PlayBtn = Driver->FindElement(By::Id("PlayButton")); //working with slate requires adding the slate HUD to gamemode defaults and removing umg from level bp
                FDriverElementRef PlayBtn = Driver->FindElement(By::Path("#PlayButton"));
                if (PlayBtn->Exists())
                {
                    PlayBtn->Click();
                    TestEqual("Button is interactable", PlayBtn->IsInteractable(), true);
                }
                else
                {
                    AddError("Error, Button doesn't exist");
                }
                });
            });


        It("Should have loaded the FPSMap.", [this]() {
            UWorld* world = nullptr;
            if (GEngine) world = GEngine->GameViewport->GetWorld();
            if (world)
            {
                FString mapname = UGameplayStatics::GetCurrentLevelName(world, true);
                TestEqual("Main Menu is Loaded", mapname, "FPSMap");
            }
            });
    });
}