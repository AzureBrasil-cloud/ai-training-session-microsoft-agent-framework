import axios, { AxiosError } from 'axios'

export interface CarSale {
  id: string
  model: string
  licensePlate: string
  color: string
  price: number
  description: string
  strengths: string[]
  weaknesses: string[]
  createdAt: string
}

const carSalesService = {
  getAll: async (): Promise<CarSale[]> => {
    try {
      const response = await axios.get<CarSale[]>('/api/carsales')
      return response.data
    } catch (error: unknown) {
      const axiosError = error as AxiosError
      throw axiosError?.response?.data
    }
  }
}

export default carSalesService

