# AutomationsApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createAutomation**](#createautomation) | **POST** /Automations | |
|[**deleteAutomationById**](#deleteautomationbyid) | **DELETE** /Automations/{id} | |
|[**getAutomation**](#getautomation) | **GET** /Automations | |
|[**getAutomationById**](#getautomationbyid) | **GET** /Automations/{id} | |
|[**getAutomationByIdTask**](#getautomationbyidtask) | **GET** /Automations/{id}/tasks | |
|[**updateAutomationById**](#updateautomationbyid) | **PUT** /Automations/{id} | |
|[**updateAutomationByIdTurnoff**](#updateautomationbyidturnoff) | **PUT** /Automations/{id}/turnoff | |
|[**updateAutomationByIdTurnon**](#updateautomationbyidturnon) | **PUT** /Automations/{id}/turnon | |

# **createAutomation**
> createAutomation()


### Example

```typescript
import {
    AutomationsApi,
    Configuration,
    AutomationDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let body: AutomationDto; // (optional)

const { status, data } = await apiInstance.createAutomation(
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **AutomationDto**|  | |


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

# **deleteAutomationById**
> deleteAutomationById()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteAutomationById(
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

# **getAutomation**
> AutomatResponseList getAutomation()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getAutomation(
    order,
    page,
    pageSize,
    orderBy,
    title
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


### Return type

**AutomatResponseList**

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

# **getAutomationById**
> Automat getAutomationById()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getAutomationById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Automat**

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

# **getAutomationByIdTask**
> TaskResponseList getAutomationByIdTask()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getAutomationByIdTask(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**TaskResponseList**

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

# **updateAutomationById**
> updateAutomationById()


### Example

```typescript
import {
    AutomationsApi,
    Configuration,
    AutomationDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)
let body: AutomationDto; // (optional)

const { status, data } = await apiInstance.updateAutomationById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **AutomationDto**|  | |
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

# **updateAutomationByIdTurnoff**
> updateAutomationByIdTurnoff()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateAutomationByIdTurnoff(
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

# **updateAutomationByIdTurnon**
> updateAutomationByIdTurnon()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateAutomationByIdTurnon(
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

