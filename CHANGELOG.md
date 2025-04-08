# Changelog

All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [2.3.0] - 2025.04.09

## Added
- LinXmlAutoGenerator class added for generating XML files to prevent code stripping issues
- ZeroQueryParams class now handles all url parameter extractions
- PlayerZeroSdk now auto caches avatarId in playerPrefs

## [2.2.0] - 2025.03.27

## Added
- SDK version and Sdk platform properties added to AvatarSessionStarted
- DeviceContext data added to AvatarSessionStarted
- DeviceID is now generated and sent in AvatarSessionStarted for webGl builds
- lastActivityAt is now sent in AvatarSessionHeartbeatEvent for idle detection
- DeeplinkHandler class added for handling deeplinks
- OnHotLoadedAvatarIdChanged added to PlayerZeroSDKManager 

## [1.1.0] - 2025.01.02

## Added
- CharacterLoaderConfig class for configuring character loading settings

## [1.0.0] - 2024.12.06

## Added
- BlueprintApi for requesting blueprints

## Updated
- Cancellation tokens now added as optional parameter to web requests and other async methods

## Removed
- All samples removed from the package
- Cleanup and removal of all unnecessary classes and functions

## [0.1.0] - 2024.27.09

First release of the PlayerZero SDK

## Added
- Style Manager and Cache Creator editor windows
- Web API wrapper classes
- Avatar loading and creation functionality
- Player Locomotion Sample
- Basic UI Sample
- Avatar Creator Sample
