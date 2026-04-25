# StoriesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createStorie**](#createstorie) | **POST** /Stories | |
|[**createStorieImport**](#createstorieimport) | **POST** /Stories/import | |
|[**deleteStorieById**](#deletestoriebyid) | **DELETE** /Stories/{id} | |
|[**getStorie**](#getstorie) | **GET** /Stories | |
|[**getStorieById**](#getstoriebyid) | **GET** /Stories/{id} | |
|[**getStorieByIdDraft**](#getstoriebyiddraft) | **GET** /Stories/{id}/draft | |
|[**getStorieByIdExport**](#getstoriebyidexport) | **GET** /Stories/{id}/export | |
|[**getStorieByIdSummary**](#getstoriebyidsummary) | **GET** /Stories/{id}/summary | |
|[**getStorieDraft**](#getstoriedraft) | **GET** /Stories/draft | |
|[**updateStorieById**](#updatestoriebyid) | **PUT** /Stories/{id} | |
|[**updateStorieByIdEnd**](#updatestoriebyidend) | **PUT** /Stories/{id}/end | |
|[**updateStorieByIdFirebase**](#updatestoriebyidfirebase) | **PUT** /Stories/{id}/firebase | |
|[**updateStorieByIdStart**](#updatestoriebyidstart) | **PUT** /Stories/{id}/start | |
|[**updateStorieByIdSummary**](#updatestoriebyidsummary) | **PUT** /Stories/{id}/summary | |

# **createStorie**
> createStorie()


### Example

```typescript
import {
    StoriesApi,
    Configuration,
    StoryDto
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let body: StoryDto; // (optional)

const { status, data } = await apiInstance.createStorie(
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **StoryDto**|  | |


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **createStorieImport**
> createStorieImport()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let file: string; // (default to undefined)
let converterType: number; // (optional) (default to undefined)
let externalUrl: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.createStorieImport(
    file,
    converterType,
    externalUrl
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **file** | [**string**] |  | defaults to undefined|
| **converterType** | [**number**] |  | (optional) defaults to undefined|
| **externalUrl** | [**string**] |  | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteStorieById**
> deleteStorieById()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteStorieById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getStorie**
> StoryResponseList getStorie()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let startFrom: string; // (optional) (default to undefined)
let startTo: string; // (optional) (default to undefined)
let endFrom: string; // (optional) (default to undefined)
let endTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getStorie(
    order,
    page,
    pageSize,
    orderBy,
    title,
    startFrom,
    startTo,
    endFrom,
    endTo
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **order** | [**string**] |  | defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **orderBy** | [**string**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **startFrom** | [**string**] |  | (optional) defaults to undefined|
| **startTo** | [**string**] |  | (optional) defaults to undefined|
| **endFrom** | [**string**] |  | (optional) defaults to undefined|
| **endTo** | [**string**] |  | (optional) defaults to undefined|


### Return type

**StoryResponseList**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getStorieById**
> Story getStorieById()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getStorieById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Story**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getStorieByIdDraft**
> Story getStorieByIdDraft()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getStorieByIdDraft(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Story**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getStorieByIdExport**
> File getStorieByIdExport()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getStorieByIdExport(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**File**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getStorieByIdSummary**
> File getStorieByIdSummary()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getStorieByIdSummary(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**File**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getStorieDraft**
> StoryResponseList getStorieDraft()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let startFrom: string; // (optional) (default to undefined)
let startTo: string; // (optional) (default to undefined)
let endFrom: string; // (optional) (default to undefined)
let endTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getStorieDraft(
    order,
    page,
    pageSize,
    orderBy,
    title,
    startFrom,
    startTo,
    endFrom,
    endTo
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **order** | [**string**] |  | defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **orderBy** | [**string**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **startFrom** | [**string**] |  | (optional) defaults to undefined|
| **startTo** | [**string**] |  | (optional) defaults to undefined|
| **endFrom** | [**string**] |  | (optional) defaults to undefined|
| **endTo** | [**string**] |  | (optional) defaults to undefined|


### Return type

**StoryResponseList**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateStorieById**
> updateStorieById()


### Example

```typescript
import {
    StoriesApi,
    Configuration,
    StoryDto
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)
let body: StoryDto; // (optional)

const { status, data } = await apiInstance.updateStorieById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **StoryDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateStorieByIdEnd**
> updateStorieByIdEnd()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateStorieByIdEnd(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateStorieByIdFirebase**
> updateStorieByIdFirebase()


### Example

```typescript
import {
    StoriesApi,
    Configuration,
    SummaryModel
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)
let body: SummaryModel; // (optional)

const { status, data } = await apiInstance.updateStorieByIdFirebase(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **SummaryModel**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateStorieByIdStart**
> updateStorieByIdStart()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateStorieByIdStart(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateStorieByIdSummary**
> updateStorieByIdSummary()


### Example

```typescript
import {
    StoriesApi,
    Configuration,
    SummaryModel
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)
let body: SummaryModel; // (optional)

const { status, data } = await apiInstance.updateStorieByIdSummary(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **SummaryModel**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

