//[assembly: LevelOfParallelism(3)] - sets the assembly to use level 3 of parallelism, meaning itwilluse three threads. ifnot set the defaul will be 2 or what the processor can handle if more than 2.
//[assembly: NonTestAssembly] - specifies this assembly doesnt have any tests within it.
// [assembly: Apartment(ApartmentState.STA)]- Setting the assembly to STA (Single Threaded Apartment) will set all tests in the assembly to run in the STA
//[assembly: Apartment(ApartmentState.MTA)] - Setting the assembly to MTA (MultiThreaded Apartment)
//                                            will set all tests in the assembly to run in the MTA (default behaviour when no specified)
//                                            Apartment state can also be set for testfixtures and individual tests.