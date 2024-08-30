#include "Tests/AutomationEditorCommon.h" // required for FAutomationEditorCommonUtils along with the "UnrealEd" module
#include "Developer/AutomationDriver/Public/AutomationDriverCommon.h" // required for IAutomationDriverModule along with the, "AutomationDriver" module



IMPLEMENT_SIMPLE_AUTOMATION_TEST(InteractableResourcesSpawnedTest, "AutomationDriver.MainMenu.NewGame", EAutomationTestFlags::EditorContext | EAutomationTestFlags::ProductFilter)

DEFINE_LATENT_AUTOMATION_COMMAND(FDisableDriver);
DEFINE_LATENT_AUTOMATION_COMMAND_THREE_PARAMETER(FFindElement, FString, ElementsId, FAutomationDriverPtr, Driver, FDriverElementRef*, Element);
DEFINE_LATENT_AUTOMATION_COMMAND_TWO_PARAMETER(FClickElement, FAutomationDriverPtr, Driver, FDriverElementRef&, Element);

bool FClickElement::Update()
{
	if (Element->Exists())
	{
		UE_LOG(LogTemp, Warning, TEXT("Test game button exists"));
		Element->Click();
		return true;
	}
	else 
	{
		return false;
	}
}

bool FFindElement::Update()
{
	FDriverElementRef btn = Driver->FindElement(By::Id(ElementsId));
	Element = &btn;
	return true;
}

bool FDisableDriver::Update()
{
	IAutomationDriverModule::Get().Disable();
	return true;
}

bool InteractableResourcesSpawnedTest::RunTest(const FString& Parameters)
{
	//bool result = false;

	//FString MapPath = FPaths::ProjectContentDir() / TEXT("Maps/MainMenu.umap");
	FAutomationEditorCommonUtils::LoadMap("/Game/Maps/MainMenu.MainMenu");

	IAutomationDriverModule::Get().Enable();

	FAutomationDriverPtr Driver = IAutomationDriverModule::Get().CreateDriver();

	// was unable to find UI element, unsure but might need the UI created in C++ first in order to add metadata to the element 
	
		//FDriverElementRef NewGameBtn = Driver->FindElement(By::Id("BTN_NewGame"));
		//TArray<FDriverElementRef> Elements= 
		// 
		//FDriverElementRef QuitBtn = Driver->FindElement(By::Path("/Game/UI/MainMenuWidget.MainMenuWidget_C/Exit_Button"));
	FDriverElementRef* BtnElement = nullptr;

	ADD_LATENT_AUTOMATION_COMMAND(FFindElement("TestButton", Driver, BtnElement));
		
	//ADD_LATENT_AUTOMATION_COMMAND(FClickElement(Driver, BtnElement&));

	ADD_LATENT_AUTOMATION_COMMAND(FDisableDriver);

	return true;
}

