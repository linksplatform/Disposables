cmake_minimum_required(VERSION 3.15)

set(LINKS_PLATFORM_TESTS ON CACHE BOOL "Whether to compile tests")

project(Platform.Disposables CXX)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
list(APPEND CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR})

# Find required packages
find_package(GTest REQUIRED)
find_package(Platform.Delegates)
find_package(Platform.Exceptions)

# Create interface library
add_library(${PROJECT_NAME}.Library INTERFACE)
target_include_directories(${PROJECT_NAME}.Library INTERFACE ${PROJECT_NAME})

# Link dependencies if available
if(Platform.Delegates_FOUND)
    target_link_libraries(${PROJECT_NAME}.Library INTERFACE Platform.Delegates::Platform.Delegates)
endif()

if(Platform.Exceptions_FOUND)
    target_link_libraries(${PROJECT_NAME}.Library INTERFACE Platform.Exceptions::Platform.Exceptions)
endif()

# Conditionally build tests
if(${LINKS_PLATFORM_TESTS})
    # Main tests
    add_executable(${PROJECT_NAME}.Tests 
        ${PROJECT_NAME}.Tests/AllTests.cpp
    )
    set_target_properties(${PROJECT_NAME}.Tests PROPERTIES CXX_STANDARD 20)
    target_link_libraries(${PROJECT_NAME}.Tests PRIVATE GTest::gtest)
    target_link_libraries(${PROJECT_NAME}.Tests PRIVATE GTest::gtest_main)
    target_link_libraries(${PROJECT_NAME}.Tests PRIVATE ${PROJECT_NAME}.Library)

    # Disposal order test executable
    add_executable(${PROJECT_NAME}.Tests.DisposalOrderTest 
        ${PROJECT_NAME}.Tests.DisposalOrderTest/Program.cpp
    )
    set_target_properties(${PROJECT_NAME}.Tests.DisposalOrderTest PROPERTIES CXX_STANDARD 20)
    target_link_libraries(${PROJECT_NAME}.Tests.DisposalOrderTest PRIVATE ${PROJECT_NAME}.Library)
endif()