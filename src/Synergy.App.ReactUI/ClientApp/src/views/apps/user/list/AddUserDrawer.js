// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Drawer from '@mui/material/Drawer'
import Select from '@mui/material/Select'
import Button from '@mui/material/Button'
import MenuItem from '@mui/material/MenuItem'
import { styled } from '@mui/material/styles'
import TextField from '@mui/material/TextField'
import IconButton from '@mui/material/IconButton'
import InputLabel from '@mui/material/InputLabel'
import Typography from '@mui/material/Typography'
import Box from '@mui/material/Box'
import FormControl from '@mui/material/FormControl'
import FormHelperText from '@mui/material/FormHelperText'

// ** Third Party Imports
import * as yup from 'yup'
import { yupResolver } from '@hookform/resolvers/yup'
import { useForm, Controller } from 'react-hook-form'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Store Imports
import { useDispatch } from 'react-redux'

// ** Actions Imports
import { addUser } from 'src/store/apps/user'

//** Axios */
import axios from 'axios'

const showErrors = (field, valueLen, min) => {
  if (valueLen === 0) {
    return `${field} field is required`
  } else if (valueLen > 0 && valueLen < min) {
    return `${field} must be at least ${min} characters`
  } else {
    return ''
  }
}

const Header = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  padding: theme.spacing(3, 4),
  justifyContent: 'space-between',
  backgroundColor: theme.palette.background.default
}))

const schema = yup.object().shape({
  company: yup.string().required(),
  country: yup.string().required(),
  email: yup.string().email().required(),
  contact: yup
    .number()
    .typeError('Contact Number field is required')
    .min(10, obj => showErrors('Contact Number', obj.value.length, obj.min))
    .required(),
  fullName: yup
    .string()
    .min(3, obj => showErrors('First Name', obj.value.length, obj.min))
    .required(),
  username: yup
    .string()
    .min(3, obj => showErrors('Username', obj.value.length, obj.min))
    .required()
})

const defaultValues = {
  id: '',
  createdDate: '',
  createdBy: '',
  lastUpdatedDate: '',
  lastUpdatedBy: '',
  isDeleted: Number('')
}

const SidebarAddUser = props => {
  // ** Props
  const { open, toggle } = props

  // ** State
  const [plan, setPlan] = useState('basic')
  const [role, setRole] = useState('subscriber')

  // ** Hooks
  const dispatch = useDispatch()

  const {
    reset,
    control,
    setValue,
    handleSubmit,
    formState: { errors }
  } = useForm({
    defaultValues,
    mode: 'onChange',
    resolver: yupResolver(schema)
  })

  const onSubmit = data => {
    dispatch(addUser({ ...data, role, currentPlan: plan }))
    toggle()
    reset()
  }

  const handleClose = () => {
    setPlan('basic')
    setRole('subscriber')
    setValue('contact', Number(''))
    toggle()
    reset()
  }

  // single state for API

    const [fullname, setFullname] = useState({
      id: '',
      createdDate: '',
      createdBy: '',
      lastUpdatedDate: '',
      lastUpdatedBy: '',
      isDeleted: ''
    })

    const handleChange = (e) => {
      const { name, value } = e.target;
      setFullname((preValue) => {
        return {
          ...preValue,
          [name]: value
        }
      })
    }

    const submitData = async (e) => {
      e.preventDefault()
      const data = {
        id: fullname.id,
        createdDate: fullname.createdDate,
        createdBy: fullname.createdBy,
        lastUpdatedDate: fullname.lastUpdatedDate,
        lastUpdatedBy: fullname.lastUpdatedBy,
        isDeleted: fullname.isDeleted


      }
      const configs = {
        "Content-Type": "application/json"
      }
      let response = await axios.post(`https://webapidev.aitalkx.com/api/Command/ManageWorkspace`, data, configs)
      console.log('response', response);


      setFullname({
        id: '',
        createdDate: '',
        createdBy: '',
        lastUpdatedDate: '',
        lastUpdatedBy: '',
        isDeleted: ''
      })
    }

    return (
      <Drawer
        open={open}
        anchorEI='anchorEI'
        variant='temporary'
        onClose={handleClose}
        ModalProps={{ keepMounted: true }}
        sx={{ '& .MuiDrawer-paper': { width: { xs: 300, sm: 400 } } }}
      >
        <Header>
          <Typography variant='h6'>Add</Typography>
          <IconButton size='small' onClick={handleClose} sx={{ color: 'text.primary' }}>
            <Icon icon='mdi:close' fontSize={20} />
          </IconButton>
        </Header>
        <Box sx={{ p: 5 }}>
          <form onSubmit={handleSubmit(onSubmit)}>
            <FormControl fullWidth sx={{ mb: 6 }}>
              <Controller
                name='id'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <TextField
                    type='text'
                    name='id'
                    value={fullname.id}
                    label='Id'
                    onChange={handleChange}
                    placeholder='Id'
              
                    // error={Boolean(errors.fullName)}
                  />
                )}
              />
              {/* {errors.fullName && <FormHelperText sx={{ color: 'error.main' }}>{errors.fullName.message}</FormHelperText>} */}
            </FormControl>
            <FormControl fullWidth sx={{ mb: 6 }}>
              <Controller
                name='createdDate'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <TextField
                  type="date"
                  name='createdDate'
                    value={fullname.createdDate}
                    // label='CreatedDate'
                    onChange={handleChange}
                    // placeholder='CreatedDate'

                    //error={Boolean(errors.username)}
                  />
                )}
              />
              {/* {errors.username && <FormHelperText sx={{ color: 'error.main' }}>{errors.username.message}</FormHelperText>} */}
            </FormControl>
            <FormControl fullWidth sx={{ mb: 6 }}>
              <Controller
                name='createdBy'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <TextField
                    type='text'
                    name='createdBy'
                    value={fullname.createdBy}
                    label='CreatedBy'
                    onChange={handleChange}
                    placeholder=''

                    //error={Boolean(errors.email)}
                  />
                )}
              />
              {/* {errors.email && <FormHelperText sx={{ color: 'error.main' }}>{errors.email.message}</FormHelperText>} */}
            </FormControl>
            <FormControl fullWidth sx={{ mb: 6 }}>
              <Controller
                name='lastUpdatedDate'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <TextField
                  type="date"
                  name='lastUpdatedDate'
                    value={fullname.lastUpdatedDate}
                    // label='lastUpdatedDate'
                    onChange={handleChange}
                    // placeholder='LastUpdatedDate'

                    //error={Boolean(errors.company)}
                  />
                )}
              />
              {/* {errors.company && <FormHelperText sx={{ color: 'error.main' }}>{errors.company.message}</FormHelperText>} */}
            </FormControl>
            <FormControl fullWidth sx={{ mb: 6 }}>
              <Controller
                name='lastUpdatedBy'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <TextField
                  type='text'
                  name='lastUpdatedBy'
                    value={fullname.lastUpdatedBy}
                    label='LastUpdatedBy'
                    onChange={handleChange}
                    placeholder='LastUpdatedBy'

                    //error={Boolean(errors.country)}
                  />
                )}
              />
              {/* {errors.country && <FormHelperText sx={{ color: 'error.main' }}>{errors.country.message}</FormHelperText>} */}
            </FormControl>
            {/* <FormControl fullWidth sx={{ mb: 6 }}>
            <Controller
              name='contact'
              control={control}
              rules={{ required: true }}
              render={({ field: { value, onChange } }) => (
                <TextField
                  type='number'
                  value={value}
                  label='Contact'
                  onChange={onChange}
                  placeholder='(397) 294-5153'
                  error={Boolean(errors.contact)}
                />
              )}
            />
            {errors.contact && <FormHelperText sx={{ color: 'error.main' }}>{errors.contact.message}</FormHelperText>}
          </FormControl>
          <FormControl fullWidth sx={{ mb: 6 }}>
            <InputLabel id='role-select'>Select Role</InputLabel>
            <Select
              fullWidth
              value={role}
              id='select-role'
              label='Select Role'
              labelId='role-select'
              onChange={e => setRole(e.target.value)}
              inputProps={{ placeholder: 'Select Role' }}
            >
              <MenuItem value='admin'>Admin</MenuItem>
              <MenuItem value='author'>Author</MenuItem>
              <MenuItem value='editor'>Editor</MenuItem>
              <MenuItem value='maintainer'>Maintainer</MenuItem>
              <MenuItem value='subscriber'>Subscriber</MenuItem>
            </Select>
          </FormControl>
          <FormControl fullWidth sx={{ mb: 6 }}>
            <InputLabel id='plan-select'>Select Plan</InputLabel>
            <Select
              fullWidth
              value={plan}
              id='select-plan'
              label='Select Plan'
              labelId='plan-select'
              onChange={e => setPlan(e.target.value)}
              inputProps={{ placeholder: 'Select Plan' }}
            >
              <MenuItem value='basic'>Basic</MenuItem>
              <MenuItem value='company'>Company</MenuItem>
              <MenuItem value='enterprise'>Enterprise</MenuItem>
              <MenuItem value='team'>Team</MenuItem>
            </Select>
          </FormControl>  */}
            <Box sx={{ display: 'flex', alignItems: 'center' }}>
              <Button size='large' type='submit' variant='contained' sx={{ mr: 3 }} onClick={submitData}>
                Submit
              </Button>
              <Button size='large' variant='outlined' color='secondary' onClick={handleClose}>
                Cancel
              </Button>
            </Box>
          </form>
        </Box>
      </Drawer>
    )
  }

  export default SidebarAddUser
