@cargo b --release --features "no-dirty-detection"
@copy target\release\waterboxhost.dll ..\..\Assets\dll
@copy target\release\waterboxhost.dll ..\..\output\dll
