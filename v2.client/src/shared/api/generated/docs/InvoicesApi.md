# InvoicesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Invoices | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Invoices/{id} | |
|[**getById**](#getbyid) | **GET** /api/Invoices/{id} | |
|[**getCollector**](#getcollector) | **GET** /api/Invoices/collector | |
|[**getRecipient**](#getrecipient) | **GET** /api/Invoices/recipient | |
|[**updateById**](#updatebyid) | **PUT** /api/Invoices/{id} | |
|[**updateByIdPositionsByPositionIdPaid**](#updatebyidpositionsbypositionidpaid) | **PUT** /api/Invoices/{id}/positions/{positionId}/paid | |

# **create**
> create()


### Example

```typescript
import {
    InvoicesApi,
    Configuration,
    SaveInvoiceDto
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let saveInvoiceDto: SaveInvoiceDto; // (optional)

const { status, data } = await apiInstance.create(
    saveInvoiceDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **saveInvoiceDto** | **SaveInvoiceDto**|  | |


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteById**
> deleteById()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteById(
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

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getById**
> getById()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getById(
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

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getCollector**
> getCollector()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

const { status, data } = await apiInstance.getCollector();
```

### Parameters
This endpoint does not have any parameters.


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getRecipient**
> getRecipient()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

const { status, data } = await apiInstance.getRecipient();
```

### Parameters
This endpoint does not have any parameters.


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateById**
> updateById()


### Example

```typescript
import {
    InvoicesApi,
    Configuration,
    SaveInvoiceDto
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)
let saveInvoiceDto: SaveInvoiceDto; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    saveInvoiceDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **saveInvoiceDto** | **SaveInvoiceDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateByIdPositionsByPositionIdPaid**
> updateByIdPositionsByPositionIdPaid()


### Example

```typescript
import {
    InvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new InvoicesApi(configuration);

let id: string; // (default to undefined)
let positionId: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdPositionsByPositionIdPaid(
    id,
    positionId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **positionId** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

